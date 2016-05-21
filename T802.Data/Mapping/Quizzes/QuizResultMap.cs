using T802.Core.Domain.Quizzes;

namespace T802.Data.Mapping.Quizzes
{
    public partial class QuizResultMap : T802EntityTypeConfiguration<QuizResult>
    {
        public QuizResultMap()
        {
            this.ToTable("QuizResult");
            this.HasKey(q => q.Id);

            this.HasRequired(qr => qr.Student)
                .WithMany()
                .HasForeignKey(qr => qr.StudentId);

            this.HasMany(c => c.Answers)
                .WithMany()
                .Map(m => m.ToTable("QuizResult_QuizUserAnswer_Mapping"));
        }
    }
}
