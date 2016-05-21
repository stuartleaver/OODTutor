namespace T802.Web.Models.Leaderboard
{
    public class LeaderboardModel : BaseT802Model
    {
        public int Position { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
    }
}