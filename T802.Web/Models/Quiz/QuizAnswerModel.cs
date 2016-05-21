using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using T802.Web.Validators.Quizzes;

namespace T802.Web.Models.Quiz
{
    public partial class QuizAnswerModel : BaseT802Model
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}