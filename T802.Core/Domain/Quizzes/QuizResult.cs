using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Domain.Students;

namespace T802.Core.Domain.Quizzes
{
    public class QuizResult : BaseEntity
    {
        private ICollection<QuizUserAnswer> _quizUserAnswers;
 
        public int StudentId { get; set; }
        public int QuizId { get; set; }
        public DateTime AnsweredOnUtc { get; set; }
        public float Score { get; set; }
        public virtual Student Student { get; set; }
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<QuizUserAnswer> Answers
        {
            get { return _quizUserAnswers ?? (_quizUserAnswers = new List<QuizUserAnswer>()); }
            set { _quizUserAnswers = value; }
        }
    }
}
