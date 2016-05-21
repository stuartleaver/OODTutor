using T802.Core.Domain.Students;

namespace T802.Data.Mapping.Students
{
    public class LevelPointsHistoryMap : T802EntityTypeConfiguration<LevelPointsHistory>
    {
        public LevelPointsHistoryMap()
        {
            this.ToTable("LevelPointsHistory");
            this.HasKey(l => l.Id);

            this.HasRequired(l => l.Student)
                .WithMany(c => c.LevelPointsHistory)
                .HasForeignKey(l => l.StudentId);

            this.HasRequired(l => l.Quiz)
                .WithMany(c => c.LevelPointsHistory)
                .HasForeignKey(l => l.QuizId);
        }
    }
}
