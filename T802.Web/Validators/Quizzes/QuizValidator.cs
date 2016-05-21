using FluentValidation;
using T802.Web.Models.Quiz;

namespace T802.Web.Validators.Quizzes
{
    public class QuizValidator : BaseT802Validator<QuizModel>
    {
        public QuizValidator()
        {
            RuleFor(model => model.Name).NotEmpty().WithMessage("Name Required");
            RuleFor(model => model.Description).NotEmpty().WithMessage("Description Required");
        }
    }
}