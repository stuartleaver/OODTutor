using T802.Core.Domain.Students;

namespace T802.Data.Mapping.Students
{
    public partial class AchievementMap : T802EntityTypeConfiguration<Achievement>
    {
        public AchievementMap()
        {
            this.ToTable("Achievement");
            this.HasKey(ach => ach.Id);
            this.Property(ach => ach.Name).IsRequired().HasMaxLength(255);
            this.Property(ach => ach.SystemName).HasMaxLength(255);
            this.Property(ach => ach.Description).HasMaxLength(500);

            this.HasRequired(al => al.AchievementLevel)
                .WithMany()
                .HasForeignKey(al => al.AchievementLevelId);
        }
    }
}
