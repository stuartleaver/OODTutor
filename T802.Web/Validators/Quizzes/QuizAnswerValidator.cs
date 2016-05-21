using FluentValidation;
using T802.Web.Models.Quiz;

namespace T802.Web.Validators.Quizzes
{
    public class QuizAnswerValidator : BaseT802Validator<QuizAnswerModel>
    {
        public QuizAnswerValidator()
        {
            RuleFor(model => model.Text).NotEmpty().WithMessage("Answer is Required");
        }
    }
}