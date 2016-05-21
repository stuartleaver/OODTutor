using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using T802.Web.Validators.Quizzes;

namespace T802.Web.Models.Quiz
{
    [Validator(typeof(QuizValidator))]
    public partial class QuizModel : BaseT802Model
    {
        public int Id { get; set; }
        public Guid QuizGuid { get; set; }
        public bool IsSystemQuiz { get; set; }
        public bool IsLevelQuiz { get; set; }
        public bool IsStudentQuiz { get; set; }
        public string SystemName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<QuizQuestionModel> Questions { get; set; }
        public int NumberOfTimesQuizTaken { get; set; }
        public int NumberOfQuizComments { get; set; }
        public int PassMark { get; set; }
        public string AchivementSystemName { get; set; }
        public string CreatedBy { get; set; }
        public bool HasTakenQuiz { get; set; }
        public float StudentScore { get; set; }
    }
}