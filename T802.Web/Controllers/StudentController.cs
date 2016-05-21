using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using T802.Core;
using T802.Core.Domain.Students;
using T802.Services.Authentication;
using T802.Services.Logging;
using T802.Services.Students;
using T802.Web.Models.Student;
using T802.Web.UI.Captcha;

namespace T802.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IStudentService _studentService;
        private readonly IStudentRegistrationService _studentRegistrationService;
        private readonly IStudentActivityService _studentActivityService;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;

        public StudentController(IAuthenticationService authenticationService,
            IStudentService studentService,
            IStudentRegistrationService studentRegistrationService,
            IStudentActivityService studentActivityService,
            IWorkContext workContext,
            IWebHelper webHelper)
        {
            _authenticationService = authenticationService;
            _studentService = studentService;
            _studentRegistrationService = studentRegistrationService;
            _studentActivityService = studentActivityService;
            _workContext = workContext;
            _webHelper = webHelper;
        }

        #region Login / logout

        public ActionResult Login()
        {
            var model = new LoginModel
            {
                RememberMe = true
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginResult = _studentRegistrationService.ValidateStudent(model.Username, model.Password);
                switch (loginResult)
                {
                    case StudentLoginResults.Successful:
                        {
                            var student = _studentService.GetStudentByUsername(model.Username);

                            //sign in new student
                            _authenticationService.SignIn(student, model.RememberMe);

                            //activity log
                            _studentActivityService.InsertActivity("Game.Login", "ActivityLog.Game.Login", student);

                            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return RedirectToAction("Index", "Home");

                            return Redirect(returnUrl);
                        }
                    case StudentLoginResults.StudentNotExist:
                        ModelState.AddModelError("", "Student Does Not Exist");
                        break;
                    case StudentLoginResults.Deleted:
                        ModelState.AddModelError("", "Deleted");
                        break;
                    case StudentLoginResults.NotActive:
                        ModelState.AddModelError("", "NotActive");
                        break;
                    case StudentLoginResults.NotRegistered:
                        ModelState.AddModelError("", "Not Registered");
                        break;
                    default:
                        ModelState.AddModelError("", "Wrong Credentials");
                        break;
                }
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Logout()
        {
            //activity log
            _studentActivityService.InsertActivity("Game.Logout", "ActivityLog.Game.Logout");

            _authenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            //check whether registration is allowed
            if (AppSettings.Get<bool>("UserRegistrationDisabled"))
                return RedirectToAction("RegisterResult", "Student", new { resultId = (int)UserRegistrationType.Disabled });

            var model = new RegisterModel();

            return View(model);
        }

        [HttpPost]
        [CaptchaValidator]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, string returnUrl, bool captchaValid)
        {
            //check whether registration is allowed
            if (AppSettings.Get<bool>("UserRegistrationDisabled"))
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            if (_workContext.CurrentStudent.IsRegistered())
            {
                //Already registered customer. 
                _authenticationService.SignOut();
            }

            //Create a new record
            _workContext.CurrentStudent = CreateNewStudentObject();

            var student = _workContext.CurrentStudent;

            //validate CAPTCHA
            if (AppSettings.Get<bool>("CaptchaEnabled") && !captchaValid)
            {
                ModelState.AddModelError("", "Wrong Captcha");
            }

            if (ModelState.IsValid)
            {
                model.Username = model.Username.Trim();

                var registrationRequest = new StudentRegistrationRequest(_workContext.CurrentStudent ?? new Student(), model.Username, model.Password);
                var registrationResult = _studentRegistrationService.RegisterStudent(registrationRequest);
                if (registrationResult.Success)
                {
                    //activity log
                    _studentActivityService.InsertActivity("Game.Registration", "ActivityLog.Game.Registration", student);

                    //login student now
                    _authenticationService.SignIn(student, true);

                    //send customer welcome message
                    var redirectUrl = Url.Action("RegisterResult", new { resultId = (int)UserRegistrationType.Standard });
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        redirectUrl = _webHelper.ModifyQueryString(redirectUrl, "returnurl=" + HttpUtility.UrlEncode(returnUrl), null);
                    return Redirect(redirectUrl);
                }

                //errors
                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult RegisterResult(int resultId)
        {
            var resultText = "";
            switch ((UserRegistrationType)resultId)
            {
                case UserRegistrationType.Disabled:
                    resultText = "Registration is disabled";
                    break;
                case UserRegistrationType.Standard:
                    resultText = "Standard";
                    break;
                case UserRegistrationType.AdminApproval:
                    resultText = "Admin Approval";
                    break;
                default:
                    break;
            }
            var model = new RegisterResultModel
            {
                Result = resultText
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CheckUsernameAvailability(string username)
        {
            var usernameAvailable = false;
            var statusText = "Not Available";

            if (_workContext.CurrentStudent != null &&
                    _workContext.CurrentStudent.Username != null &&
                    _workContext.CurrentStudent.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
            {
                statusText = "Current Username";
            }
            else
            {
                var student = _studentService.GetStudentByUsername(username);
                if (student == null)
                {
                    statusText = "Available";
                    usernameAvailable = true;
                }
            }

            return Json(new { Available = usernameAvailable, Text = statusText });
        }

        public ActionResult PasswordRecovery()
        {
            var model = new PasswordRecoveryModel();

            //activity log
            _studentActivityService.InsertActivity("Game.PasswordRecovery", "ActivityLog.Game.PasswordRecovery");

            return View(model);
        }

        [HttpPost]
        public ActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            var student = _studentService.GetStudentByUsername(model.Username);

            if (student == null && !student.StudentRoles.Contains(_studentService.GetStudentRoleBySystemName(SystemStudentRoleNames.Administrators)))
                ModelState.AddModelError("", "Username not found");

            if (ModelState.IsValid)
            {
                var response = _studentRegistrationService.ChangePassword(new ChangePasswordRequest(model.Username,
                        false, model.Password));

                model.Result = "Password has been changed.";
                model.SuccessfullyChanged = true;

                //activity log
                _studentActivityService.InsertActivity("Game.PasswordRecovery.Success", "ActivityLog.Game.PasswordRecovery.Success");

                return View(model);
            }

            //If we got this far, something failed, redisplay form

            //activity log
            _studentActivityService.InsertActivity("Game.PasswordRecovery.Fail", "ActivityLog.Game.PasswordRecovery.Fail");

            return View(model);
        }

        public ActionResult Survey()
        {
            //activity log
            _studentActivityService.InsertActivity("Game.Survey", "ActivityLog.Game.Survey");

            return Redirect("https://www.surveymonkey.com/r/oodtutor-finalsurvey");
        }

        [NonAction]
        public virtual Student CreateNewStudentObject()
        {
            var student = new Student
            {
                StudentGuid = Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
            };

            return student;
        }

        #endregion
    }
}