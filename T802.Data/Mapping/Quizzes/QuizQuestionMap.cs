using T802.Core.Domain.Quizzes;

namespace T802.Data.Mapping.Quizzes
{
    public partial class QuizQuestionMap : T802EntityTypeConfiguration<QuizQuestion>
    {
        public QuizQuestionMap()
        {
            this.ToTable("QuizQuestion");
            this.HasKey(qq => qq.Id);

            this.HasRequired(qq => qq.Quiz)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuizId);
        }
    }
}
