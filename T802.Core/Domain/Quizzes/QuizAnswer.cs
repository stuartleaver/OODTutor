using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T802.Core.Domain.Quizzes
{
    public partial class QuizAnswer : BaseEntity
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public virtual QuizQuestion QuizQuestion { get; set; }
    }
}
