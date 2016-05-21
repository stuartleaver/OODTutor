using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using T802.Core;
using T802.Core.Domain.Students;
using T802.Services.Authentication.Security;
using T802.Services.Students;

namespace T802.Services.Authentication
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        private readonly IStudentService _studentService;
        private readonly TimeSpan _expirationTimeSpan;

        private Student _cachedStudent;

        public FormsAuthenticationService(HttpContextBase httpContext, IStudentService studentService)
        {
            this._httpContext = httpContext;
            this._studentService = studentService;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
        }

        public void SignIn(Student student, bool createPersistentCookie)
        {

            var webWorkContect = DependencyResolver.Current.GetService<IWorkContext>();
            
            
            var now = DateTime.UtcNow.ToLocalTime();

            var serializeModel = new StudentPrincipleSerializeModel { Id = student.Id, Username = student.Username, Roles = student.StudentRoles};
            var serializer = new JavaScriptSerializer();
            var userData = serializer.Serialize(serializeModel);

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                student.Username,
                now,
                createPersistentCookie ? now.AddDays(7) : now.Add(_expirationTimeSpan),
                createPersistentCookie,
                userData);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            _cachedStudent = student;
        }

        public void SignOut()
        {
            _cachedStudent = null;
            FormsAuthentication.SignOut();
        }

        public Student GetAuthenticatedStudent()
        {
            if (_cachedStudent != null)
                return _cachedStudent;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is GenericIdentity))
            {
                return null;
            }

            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            var student = GetAuthenticatedCustomerFromTicket(authTicket);
            if (student != null && student.Active && !student.Deleted && student.IsRegistered())
                _cachedStudent = student;
            return _cachedStudent;
        }

        public virtual Student GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            StudentPrincipleSerializeModel serializeModel = serializer.Deserialize<StudentPrincipleSerializeModel>(ticket.UserData);

            var usernameOrEmail = serializeModel.Username;

            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;
            var student = _studentService.GetStudentByUsername(usernameOrEmail);
            return student;
        }
    }
}
