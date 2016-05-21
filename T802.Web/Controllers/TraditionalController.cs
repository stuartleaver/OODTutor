using System.Web.Mvc;
using T802.Core.Domain.Students;
using T802.Services.Achievements;
using T802.Services.Logging;
using T802.Services.Quizzes;
using T802.Services.Students;
using T802.Web.Framework;
using T802.Web.Models.Traditional;

namespace T802.Web.Controllers
{
    [CustomAuthorize("TraditionalMethod")]
    public class TraditionalController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IAchievementService _achievementService;
        private readonly IStudentActivityService _studentActivityService;
        private readonly IQuizService _quizService;

        public TraditionalController(IStudentService studentService,
            IAchievementService achievementService,
            IStudentActivityService studentActivityService,
            IQuizService quizService)
        {
            this._studentService = studentService;
            this._achievementService = achievementService;
            this._studentActivityService = studentActivityService;
            this._quizService = quizService;
        }

        public ActionResult Index()
        {
            var model = new TraditionalHomeModel();
            PrepareHomeModel(model);

            //activity log
            _studentActivityService.InsertActivity("Home.Page.Traditional", "ActivityLog.Home.Page.Traditional");

            return View(model);
        }

        private void PrepareHomeModel(TraditionalHomeModel model)
        {
            var student = _studentService.GetStudentByUsername(User.Identity.Name);

            model.TakenInitialQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.CompletedInitialQuiz,
                    student.Id);

            model.TakenFinalQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.CompletedFinalQuiz,
                    student.Id);
        }

        public ActionResult Quiz(string id)
        {
            var quizId = _quizService.GetQuizBySystemName(id).Id;
            return RedirectToAction("Quiz", "Quizzes", new { id = quizId });
        }
    }
}