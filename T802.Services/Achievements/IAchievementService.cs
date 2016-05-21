using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Domain.Students;

namespace T802.Services.Achievements
{
    public interface IAchievementService
    {
        /// <summary>
        /// Gets list of achievements
        /// </summary>
        /// <returns>IList Achievement</returns>
        IList<Achievement> GetAchievementList();

        /// <summary>
        /// Gets a achievement
        /// </summary>
        /// <param name="systemName">Achievement system name</param>
        /// <returns>Achievement</returns>
        Achievement GetAchievementBySystemName(string systemName);

        /// <summary>
        /// Gets list of achievements a student has
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>IList Achievement</returns>
        IList<Achievement> GetStudentAchievementList(int studentId);

        /// <summary>
        /// Checks to see if a student has an achievement
        /// </summary>
        /// <param name="achievement">Achievement</param>
        /// <param name="studentId">Student Id</param>
        /// <returns>Boolean</returns>
        bool HasAchievementByAchievement(Achievement achievement, int studentId);

        /// <summary>
        /// Checks to see if a student has an achievement
        /// </summary>
        /// <param name="achievementId">Achievement system name</param>
        /// <param name="studentId">Student Id</param>
        /// <returns>Boolean</returns>
        bool HasAchievementBySystemName(string achievementId, int studentId);
    }
}
