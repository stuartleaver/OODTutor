using System.Collections.Generic;
using T802.Core.Domain.Leaderboard;

namespace T802.Services.Leaderboards
{
    public interface ILeaderboardService
    {
        IList<Leaderboard> GetMainLeaderboard();
        IList<Leaderboard> GetLeaderboard(string quiz);
        IList<AchievementLeaderboard> GetAchievementLeaderboard();
    }
}
