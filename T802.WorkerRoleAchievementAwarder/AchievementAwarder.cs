using System.Linq;
using T802.Core.Domain.Students;
using T802.Services.Achievements;
using T802.Services.Students;

namespace T802.WorkerRoleAchievementAwarder
{
    public interface IAchievementAwarder
    {
        void AwardAchievements(Student student);
    }

    public class AchievementAwarder : IAchievementAwarder
    {
        private readonly IAchievementService _achievementService;
        private readonly IStudentService _studentService;

        public AchievementAwarder(IAchievementService achievementService,
            IStudentService studentService)
        {
            this._achievementService = achievementService;
            this._studentService = studentService;
        }

        public void AwardAchievements(Student student)
        {
            ProcessPlatinumAchievement(student);
        }

        private void ProcessPlatinumAchievement(Student student)
        {
            var achievements = _achievementService.GetAchievementList();
            var studentAchievements = _achievementService.GetStudentAchievementList(student.Id);

            // Remove platinum achievement from comparison
            achievements.Remove(
                _achievementService.GetAchievementBySystemName(SystemStudentAchievementNames.EarntAllAchievements));

            var comparisionList = achievements.Except(studentAchievements).ToList();
            if (comparisionList.Count == 0)
            {
                student.AddAchievementHistoryEntry(_achievementService.GetAchievementBySystemName(SystemStudentAchievementNames.EarntAllAchievements));
                _studentService.UpdateStudent(student);
            }

        }
    }
}
