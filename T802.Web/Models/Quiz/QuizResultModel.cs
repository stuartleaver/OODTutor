using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T802.Web.Models.Quiz
{
    public class QuizResultModel
    {
        public int StudentId { get; set; }
        public int QuizId { get; set; }
        public string SystemName { get; set; }
        public DateTime AnsweredOnUtc { get; set; }
        public float StudentScore { get; set; }
        public int PassMark { get; set; }
        public bool IsSystemQuiz { get; set; }
        public bool IsLevelQuiz { get; set; }
        public bool IsStudentQuiz { get; set; }
        public IList<QuizQuestionModel> Questions { get; set; }
    }
}