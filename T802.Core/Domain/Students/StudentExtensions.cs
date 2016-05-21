using System;
using System.Linq;
using System.Web;

namespace T802.Core.Domain.Students
{
    public static class StudentExtensions
    {
        /// <summary>
        /// Gets a value indicating whether student is registered
        /// </summary>
        /// <param name="student">Student</param>
        /// <param name="onlyActiveStudentRoles">A value indicating whether we should look only in active student roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this Student student, bool onlyActiveStudentRoles = true)
        {
            if (student == null) return false;
            if (IsInStudentRole(student, SystemStudentRoleNames.Administrators, onlyActiveStudentRoles) ||
                IsInStudentRole(student, SystemStudentRoleNames.Game, onlyActiveStudentRoles) ||
                IsInStudentRole(student, SystemStudentRoleNames.Traditional, onlyActiveStudentRoles))
            {
                return true;
            }
            return false;
        }

        public static bool IsInStudentRole(Student student,
            string studentRoleSystemName, bool onlyActiveStudentRoles = true)
        {
            if (student == null)
                throw new ArgumentNullException("student");

            if (String.IsNullOrEmpty(studentRoleSystemName))
                throw new ArgumentNullException("studentRoleSystemName");

            var result = student.StudentRoles
                .FirstOrDefault(cr => (!onlyActiveStudentRoles || cr.Active) && (cr.SystemName == studentRoleSystemName)) != null;
            return result;
        }

        #region Reward points

        public static void AddRewardPointsHistoryEntry(this Student student,
            int points, string message = "", decimal usedAmount = 0M)
        {
            int newPointsBalance = student.GetRewardPointsBalance() + points;

            var rewardPointsHistory = new RewardPointsHistory
            {
                Student = student,
                Points = points,
                PointsBalance = newPointsBalance,
                UsedAmount = usedAmount,
                Message = message,
                CreatedOnUtc = DateTime.UtcNow
            };

            student.RewardPointsHistory.Add(rewardPointsHistory);
        }

        /// <summary>
        /// Gets reward points balance
        /// </summary>
        public static int GetRewardPointsBalance(this Student student)
        {
            int result = 0;
            var lastRph = student.RewardPointsHistory
                    .OrderByDescending(rph => rph.CreatedOnUtc)
                    .ThenByDescending(rph => rph.Id)
                    .FirstOrDefault();
            if (lastRph != null)
                result = lastRph.PointsBalance;
            return result;
        }

        #endregion

        #region Level points

        public static void AddLevelPointsHistoryEntry(this Student student,
            int points, int quizId, string message = "")
        {
            var levelPointsHistory = new LevelPointsHistory
            {
                QuizId = quizId,
                Student = student,
                Points = points,
                Message = message,
                CreatedOnUtc = DateTime.UtcNow
            };

            student.LevelPointsHistory.Add(levelPointsHistory);
        }

        #endregion

        #region Achievements

        public static void AddAchievementHistoryEntry(this Student student,
            Achievement achievement)
        {
            if (HasAchievement(student, achievement.Id))
                return;

            var achievementHistory = new AchievementHistory()
            {
                Student = student,
                AwardedOnUtc = DateTime.UtcNow,
                AchievementId = achievement.Id
            };

            student.AchievementHistory.Add(achievementHistory);

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session["AchievementAwarded"] = 1;
                HttpContext.Current.Session["AchievementAwardedName"] = achievement.Name;
            }
        }

        private static bool HasAchievement(this Student student, int achievementId)
        {
            var result = student.AchievementHistory
               .FirstOrDefault(cr => cr.AchievementId == achievementId) != null;
            return result;
        }

        #endregion
    }
}
