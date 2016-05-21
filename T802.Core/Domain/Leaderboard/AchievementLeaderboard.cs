using T802.Core.Domain.Students;

namespace T802.Core.Domain.Leaderboard
{
    public class AchievementLeaderboard
    {
        public Student Student { get; set; }
        public int PlatinumCount { get; set; }
        public int GoldCount { get; set; }
        public int SilverCount { get; set; }
        public int BronzeCount { get; set; }

        public int TotalCount { get; set; }
    }
}
