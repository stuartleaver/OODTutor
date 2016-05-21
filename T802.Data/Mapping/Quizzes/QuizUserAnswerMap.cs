using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Domain.Quizzes;

namespace T802.Data.Mapping.Quizzes
{
    public partial class QuizUserAnswerMap : T802EntityTypeConfiguration<QuizUserAnswer>
    {
        public QuizUserAnswerMap()
        {
            this.ToTable("QuizUserAnswer");
            this.HasKey(q => q.Id);
        }
    }
}
