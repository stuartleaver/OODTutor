using System.Collections.Generic;
using System.Linq;
using T802.Core.Data;
using T802.Core.Domain.Leaderboard;
using T802.Core.Domain.Students;
using T802.Services.Students;

namespace T802.Services.Leaderboards
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly IRepository<LevelPointsHistory> _levelPointsHistoryRepository;
        private readonly IRepository<AchievementHistory> _achievementHistoryRepository;
        private readonly StudentService _studentService;

        private StudentRole StudentGameRole;

        public LeaderboardService(IRepository<LevelPointsHistory> levelPointsHistoryRepository,
            IRepository<AchievementHistory> achievementHistoryRepository,
            StudentService studentService)
        {
            this._levelPointsHistoryRepository = levelPointsHistoryRepository;
            this._achievementHistoryRepository = achievementHistoryRepository;
            this._studentService = studentService;

            this.StudentGameRole = _studentService.GetStudentRoleBySystemName(SystemStudentRoleNames.Game);
        }

        public IList<Leaderboard> GetMainLeaderboard()
        {
            var query = (from r in _levelPointsHistoryRepository.Table
                         where r.Quiz.IsLevelQuiz && r.Student.StudentRoles.Any(x => x.Id == StudentGameRole.Id)
                         group r by r.Student
                             into g
                             select new Leaderboard
                             {
                                 Student = g.Key,
                                 Score = g.Sum(row => row.Points)
                             }
               ).OrderByDescending(o => o.Score).ThenBy(o => o.Student.Username);

            return query.ToList();
        }


        public IList<Leaderboard> GetLeaderboard(string quiz)
        {
            var query = (from r in _levelPointsHistoryRepository.Table
                         where r.Quiz.IsLevelQuiz && r.Quiz.SystemName == quiz && r.Student.StudentRoles.Any(x => x.Id == StudentGameRole.Id)
                         group r by r.Student
                             into g
                             select new Leaderboard
                             {
                                 Student = g.Key,
                                 Score = g.Sum(row => row.Points)
                             }
                             ).OrderByDescending(o => o.Score).ThenBy(o => o.Student.Username);

            return query.ToList();
        }


        public IList<AchievementLeaderboard> GetAchievementLeaderboard()
        {
            var query = from a in _achievementHistoryRepository.Table
                        where a.Student.StudentRoles.Any(r => r.Id == StudentGameRole.Id)
                        group a by new { a.Student, a.Achievement.AchievementLevelId }
                            into g
                            select new AchievementLeaderboard
                            {
                                Student = g.Key.Student,
                                BronzeCount = g.Where(z => z.Achievement.AchievementLevel.AchievementLevelId == SystemAchievementLevels.Bronze).Select(x => x.AchievementId).Count(),
                                SilverCount = g.Where(z => z.Achievement.AchievementLevel.AchievementLevelId == SystemAchievementLevels.Silver).Select(x => x.AchievementId).Count(),
                                GoldCount = g.Where(z => z.Achievement.AchievementLevel.AchievementLevelId == SystemAchievementLevels.Gold).Select(x => x.AchievementId).Count(),
                                PlatinumCount = g.Where(z => z.Achievement.AchievementLevel.AchievementLevelId == SystemAchievementLevels.Platinum).Select(x => x.AchievementId).Count(),
                                TotalCount = g.Select(x => x.AchievementId).Count()
                            };

            var leaderboardQuery = from l in query
                                   group l by l.Student
                                       into g
                                       select new AchievementLeaderboard
                                       {
                                           Student = g.Key,
                                           BronzeCount = g.Sum(row => row.BronzeCount),
                                           SilverCount = g.Sum(row => row.SilverCount),
                                           GoldCount = g.Sum(row => row.GoldCount),
                                           PlatinumCount = g.Sum(row => row.PlatinumCount),
                                           TotalCount = g.Sum(row => row.TotalCount)
                                       };

            return leaderboardQuery.OrderByDescending(o => o.TotalCount)
                .ThenByDescending(o => o.PlatinumCount)
                .ThenByDescending(o => o.GoldCount)
                .ThenByDescending(o => o.SilverCount)
                .ThenByDescending(o => o.BronzeCount)
                .ThenByDescending(o => o.Student.Username)
                .ToList();
        }
    }
}
