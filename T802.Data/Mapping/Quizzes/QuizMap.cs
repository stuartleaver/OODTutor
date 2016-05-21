using T802.Core.Domain.Quizzes;

namespace T802.Data.Mapping.Quizzes
{
    public partial class QuizMap : T802EntityTypeConfiguration<Quiz>
    {
        public QuizMap()
        {
            this.ToTable("Quiz");
            this.HasKey(q => q.Id);

            this.HasRequired(q => q.CreatedBy)
                .WithMany(q => q.StudentQuizzes)
                .HasForeignKey(q => q.StudentId)
                .WillCascadeOnDelete(false);
        }
    }
}
