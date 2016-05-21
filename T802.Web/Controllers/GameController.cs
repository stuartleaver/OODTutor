using System.Web.Mvc;
using T802.Core.Domain.Students;
using T802.Services.Achievements;
using T802.Services.Logging;
using T802.Services.Quizzes;
using T802.Services.Students;
using T802.Web.Framework;
using T802.Web.Models.Game;

namespace T802.Web.Controllers
{
    [CustomAuthorize("GameMethod")]
    public class GameController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IAchievementService _achievementService;
        private readonly IQuizService _quizService;
        private readonly IStudentActivityService _studentActivityService;

        public GameController(IStudentService studentService,
            IAchievementService achievementService,
            IQuizService quizService,
            IStudentActivityService studentActivityService)
        {
            this._studentService = studentService;
            this._achievementService = achievementService;
            this._quizService = quizService;
            this._studentActivityService = studentActivityService;
        }

        public ActionResult Index()
        {
            var model = new GameHomeModel();
            PrepareGameModel(model);

            //activity log
            _studentActivityService.InsertActivity("Home.Page.Game", "ActivityLog.Home.Page.Game");

            return View(model);
        }

        [NonAction]
        private void PrepareGameModel(GameHomeModel model)
        {
            var student = _studentService.GetStudentByUsername(User.Identity.Name);

            model.TakenInitialQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.CompletedInitialQuiz,
                    student.Id);

            #region SRP

            model.PassedSRPBronzeQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.SRPBronzeLevel,
                    student.Id);

            model.PassedSRPSilverQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.SRPSilverLevel,
                    student.Id);

            model.PassedSRPGoldQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.SRPGoldLevel,
                    student.Id);

            #endregion

            #region OCP

            model.PassedOCPBronzeQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.OCPBronzeLevel,
                    student.Id);

            model.PassedOCPSilverQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.OCPSilverLevel,
                    student.Id);

            model.PassedOCPGoldQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.OCPGoldLevel,
                    student.Id);

            #endregion

            #region LSP

            model.PassedLSPBronzeQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.LSPBronzeLevel,
                    student.Id);

            model.PassedLSPSilverQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.LSPSilverLevel,
                    student.Id);

            model.PassedLSPGoldQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.LSPGoldLevel,
                    student.Id);

            #endregion

            #region ISP

            model.PassedISPBronzeQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.ISPBronzeLevel,
                    student.Id);

            model.PassedISPSilverQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.ISPSilverLevel,
                    student.Id);

            model.PassedISPGoldQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.ISPGoldLevel,
                    student.Id);

            #endregion

            #region DSP

            model.PassedDSPBronzeQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.DIPBronzeLevel,
                    student.Id);

            model.PassedDSPSilverQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.DIPSilverLevel,
                    student.Id);

            model.PassedDSPGoldQuiz =
                _achievementService.HasAchievementBySystemName(SystemStudentAchievementNames.DIPGoldLevel,
                    student.Id);

            #endregion

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