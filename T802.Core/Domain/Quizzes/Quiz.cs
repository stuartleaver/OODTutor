using System;
using System.Collections.Generic;
using T802.Core.Domain.Students;

namespace T802.Core.Domain.Quizzes
{
    public partial class Quiz : BaseEntity
    {
        private ICollection<QuizQuestion> _quizQuestions;
        private ICollection<LevelPointsHistory> _levelPointsHistory;

        public Quiz()
        {
            QuizGuid = Guid.NewGuid();
        }

        public Guid QuizGuid { get; set; }
        public string SystemName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedUtc { get; set; }
        public virtual Student CreatedBy { get; set; }
        public int StudentId { get; set; }
        public bool IsSystemQuiz { get; set; }
        public bool IsLevelQuiz { get; set; }
        public bool IsStudentQuiz { get; set; }
        public int PassMark { get; set; }
        public string AchivementSystemName { get; set; }

        public virtual ICollection<QuizQuestion> QuizQuestions
        {
            get { return _quizQuestions ?? (_quizQuestions = new List<QuizQuestion>()); }
            set { _quizQuestions = value; }
        }

        public virtual ICollection<LevelPointsHistory> LevelPointsHistory
        {
            get { return _levelPointsHistory ?? (_levelPointsHistory = new List<LevelPointsHistory>()); }
            set { _levelPointsHistory = value; }
        }
    }
}
