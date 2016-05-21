using System.Collections.Generic;

namespace T802.Core.Domain.Quizzes
{
    public partial class QuizQuestion : BaseEntity
    {
        private ICollection<QuizAnswer> _quizAnswers;

        public int QuizId { get; set; }
        public string Question { get; set; }
        public int SelectedAnswer { get; set; }
        public int Points { get; set; }
        public string Image { get; set; }
        public string Hint { get; set; }
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<QuizAnswer> QuizAnswers
        {
            get { return _quizAnswers ?? (_quizAnswers = new List<QuizAnswer>()); }
            set { _quizAnswers = value; }
        }
    }
}
