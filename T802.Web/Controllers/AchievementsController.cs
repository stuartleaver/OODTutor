using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using T802.Core.Domain.Students;
using T802.Services.Achievements;
using T802.Services.Logging;
using T802.Services.Students;
using T802.Web.Framework;
using T802.Web.Models.Achievement;

namespace T802.Web.Controllers
{
    [CustomAuthorize("GameMethod")]
    public class AchievementsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IAchievementService _achievementService;
        private readonly IStudentActivityService _studentActivityService;

        public AchievementsController(IStudentService studentService,
            IAchievementService achievementService,
            IStudentActivityService studentActivityService)
        {
            this._studentService = studentService;
            this._achievementService = achievementService;
            this._studentActivityService = studentActivityService;
        }

        public ActionResult Index(string id)
        {
            List<AchievementModel> model = new List<AchievementModel>();
            PrepareAchievementModel(model, _achievementService.GetAchievementList(), id);

            _studentActivityService.InsertActivity("Game.Achievements.List", "ActivityLog.Game.Achievements.List." + id);

            return View(model);
        }

        [NonAction]
        private void PrepareAchievementModel(List<AchievementModel> model, IList<Achievement> achievements, string username)
        {
            var studentId = _studentService.GetStudentByUsername(username).Id;

            model.AddRange(achievements.Select(achievement => new AchievementModel
            {
                AchievementLevelId = achievement.AchievementLevelId,
                Active = achievement.Active,
                Description = achievement.Description,
                Name = achievement.Name,
                SystemName = achievement.SystemName,
                Level = achievement.AchievementLevel.AchievementLevelDescription,
                HasAchievement = _achievementService.HasAchievementByAchievement(achievement, studentId)
            }));
        }
    }
}