using FluentValidation;
using T802.Web.Models.Quiz;

namespace T802.Web.Validators.Quizzes
{
    public class QuizQuestionValidator : BaseT802Validator<QuizQuestionModel>
    {
        public QuizQuestionValidator()
        {
            RuleFor(model => model.Question).NotEmpty().WithMessage("Question is Required");
            RuleFor(model => model.Points).NotEmpty().WithMessage("Number of Points is Required");
            RuleFor(model => model.Points).InclusiveBetween(1, 20).WithMessage("Points should be between 1 - 20 based on difficulty");
        }
    }
}