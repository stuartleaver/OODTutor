using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T802.Core.Domain.Quizzes
{
    public class QuizComment : BaseEntity
    {
        public int CommentId { get; set; }
        public int QuizId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedUtc { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
