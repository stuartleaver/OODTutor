using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T802.Core.Domain.Quizzes
{
    public class QuizUserAnswer : BaseEntity
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public virtual QuizResult QuizResult { get; set; }
    }
}
