using T802.Core.Domain.Students;

namespace T802.Data.Mapping.Students
{
    public partial class AchievementLevelMap : T802EntityTypeConfiguration<AchievementLevel>
    {
        public AchievementLevelMap()
        {
            this.ToTable("AchievementLevel");
            this.HasKey(c => c.AchievementLevelId);
        }
    }
}
