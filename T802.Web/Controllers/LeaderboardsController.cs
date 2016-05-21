using System.Collections.Generic;
using System.Web.Mvc;
using T802.Core.Domain.Leaderboard;
using T802.Services.Leaderboards;
using T802.Services.Logging;
using T802.Services.Quizzes;
using T802.Web.Framework;
using T802.Web.Models.Leaderboard;

namespace T802.Web.Controllers
{
    [CustomAuthorize("GameMethod")]
    public class LeaderboardsController : Controller
    {
        private readonly ILeaderboardService _leaderboardService;
        private readonly IQuizService _quizService;
        private readonly IStudentActivityService _studentActivityService;


        public LeaderboardsController(ILeaderboardService leaderboardService,
            IQuizService quizService,
            IStudentActivityService studentActivityService)
        {
            this._leaderboardService = leaderboardService;
            this._quizService = quizService;
            this._studentActivityService = studentActivityService;

        }

        public ActionResult Index(string id)
        {
            var model = new List<LeaderboardModel>();

            IList<Leaderboard> leaderboard;
            if (id == "Main")
            {
                leaderboard = _leaderboardService.GetMainLeaderboard();
                ViewBag.QuizName = "Main";
            }
            else
            {
                leaderboard = _leaderboardService.GetLeaderboard(id);
                ViewBag.QuizName = _quizService.GetQuizBySystemName(id).Name;
            }

            PrepareLeaderboardModel(model, leaderboard);

            //activity log
            _studentActivityService.InsertActivity("Game.Leaderboard", "ActivityLog.Game.Leaderboard." + id);

            return View(model);
        }

        public ActionResult Achievements()
        {
            var model = new List<AchievementLeaderboardModel>();
            var leaderboard = _leaderboardService.GetAchievementLeaderboard();
            PrepareAchievementLeaderboardModel(model, leaderboard);

            //activity log
            _studentActivityService.InsertActivity("Game.Leaderboard.Achievements", "ActivityLog.Game.Leaderboard.Achievements");

            return View(model);
        }

        [NonAction]
        private void PrepareLeaderboardModel(List<LeaderboardModel> model, IList<Leaderboard> leaderboard)
        {
            var previousPosition = 0;
            var previousScore = 0;

            foreach (var row in leaderboard)
            {
                var item = new LeaderboardModel
                {
                    Position = previousScore != row.Score ? (previousPosition += 1) : previousPosition,
                    Username = row.Student.Username,
                    Score = row.Score
                };

                previousScore = item.Score;

                model.Add(item);
            }
        }

        [NonAction]
        private void PrepareAchievementLeaderboardModel(List<AchievementLeaderboardModel> model,
            IList<AchievementLeaderboard> leaderboard)
        {
            var previousPosition = 0;
            var previousTotal = 0;

            foreach (var row in leaderboard)
            {
                var item = new AchievementLeaderboardModel
                {
                    Position = previousTotal != row.TotalCount ? (previousPosition += 1) : previousPosition,
                    Username = row.Student.Username,
                    BronzeCount = row.BronzeCount,
                    SilverCount = row.SilverCount,
                    GoldCount = row.GoldCount,
                    PlatinumCount = row.PlatinumCount,
                    TotalCount = row.TotalCount
                };

                previousTotal = item.TotalCount;

                model.Add(item);
            }
        }
    }
}