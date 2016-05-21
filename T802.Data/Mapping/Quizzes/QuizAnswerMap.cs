using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Domain.Quizzes;

namespace T802.Data.Mapping.Quizzes
{
    public partial class QuizAnswerMap : T802EntityTypeConfiguration<QuizAnswer>
    {
        public QuizAnswerMap()
        {
            this.ToTable("QuizAnswer");
            this.HasKey(qa => qa.Id);

            this.HasRequired(qa => qa.QuizQuestion)
                .WithMany(qq => qq.QuizAnswers)
                .HasForeignKey(qa => qa.QuestionId);
        }
    }
}
