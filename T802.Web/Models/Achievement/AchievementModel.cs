namespace T802.Web.Models.Achievement
{
    public class AchievementModel : BaseT802Model
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public int AchievementLevelId { get; set; }
        public bool HasAchievement { get; set; }
    }
}