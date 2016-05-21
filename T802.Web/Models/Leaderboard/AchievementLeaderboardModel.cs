namespace T802.Web.Models.Leaderboard
{
    public class AchievementLeaderboardModel : BaseT802Model
    {
        public int Position { get; set; }
        public string Username { get; set; }
        public int PlatinumCount { get; set; }
        public int GoldCount { get; set; }
        public int SilverCount { get; set; }
        public int BronzeCount { get; set; }
        public int TotalCount { get; set; }
    }
}