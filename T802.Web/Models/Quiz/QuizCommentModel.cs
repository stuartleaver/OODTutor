using System;
using FluentValidation.Attributes;
using T802.Web.Validators.Quizzes;

namespace T802.Web.Models.Quiz
{
    [Validator(typeof(QuizCommentValidator))]
    public class QuizCommentModel : BaseT802Model
    {
        public int CommentId { get; set; }
        public int QuizId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}