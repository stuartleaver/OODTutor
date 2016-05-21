using System.Collections.Generic;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using T802.Web.Validators.Quizzes;

namespace T802.Web.Models.Quiz
{
    [Validator(typeof(QuizQuestionValidator))]
    public partial class QuizQuestionModel : BaseT802Model
    {
        [Key]
        public int Id { get; set; }
        public string Question { get; set; }
        public int SelectedAnswer { get; set; }
        public int Points { get; set; }
        public string Image { get; set; }
        public string Hint { get; set; }
        public IList<QuizAnswerModel> Answers { get; set; }
        public int QuizId { get; set; }
    }
}