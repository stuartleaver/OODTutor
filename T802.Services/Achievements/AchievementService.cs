using System;
using System.Collections.Generic;
using System.Linq;
using T802.Core.Caching;
using T802.Core.Data;
using T802.Core.Domain.Students;

namespace T802.Services.Achievements
{
    public class AchievementService : IAchievementService
    {
        private const string ACHIEVEMENTS_BY_SYSTEMNAME_KEY = "T802.achievement.systemname-{0}";

        private readonly IRepository<Achievement> _achievementService;
        private readonly IRepository<AchievementHistory> _achievementHistoryServiceRepository; 
        private readonly ICacheManager _cacheManager;

        public AchievementService(IRepository<Achievement> achievementService,
            IRepository<AchievementHistory> achievementHistoryServiceRepository,
            ICacheManager cacheManager)
        {
            this._achievementService = achievementService;
            this._achievementHistoryServiceRepository = achievementHistoryServiceRepository;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// Gets list of achievements
        /// </summary>
        /// <returns>IList Achievement</returns>
        public IList<Achievement> GetAchievementList()
        {
            return _achievementService.Table.OrderByDescending(a => a.AchievementLevelId).ToList();
        }

        /// <summary>
        /// Gets a achievement
        /// </summary>
        /// <param name="systemName">Achievement system name</param>
        /// <returns>Achievement</returns>
        public Achievement GetAchievementBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            string key = string.Format(ACHIEVEMENTS_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from ach in _achievementService.Table
                            orderby ach.Id
                            where ach.SystemName == systemName
                            select ach;
                var achievement = query.FirstOrDefault();
                return achievement;
            });
        }

        /// <summary>
        /// Gets list of achievements a student has
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>IList Achievement</returns>
        public IList<Achievement> GetStudentAchievementList(int studentId)
        {
            var query = from a in _achievementHistoryServiceRepository.Table
                where a.StudentId == studentId
                select a.Achievement;

            return query.ToList();
        }

        public bool HasAchievementBySystemName(string systemName, int studentId)
        {
            var achievementId = GetAchievementBySystemName(systemName);
            if (achievementId == null)
                return false;

            var query = from a in _achievementHistoryServiceRepository.Table
                        where a.StudentId == studentId && a.AchievementId == achievementId.Id
                select a;

            return query.Any();
        }

        public bool HasAchievementByAchievement(Achievement achievement, int studentId)
        {
            if (achievement.Id == null)
                return false;

            var query = from a in _achievementHistoryServiceRepository.Table
                        where a.StudentId == studentId && a.AchievementId == achievement.Id
                        select a;

            return query.Any();
        }
    }
}
