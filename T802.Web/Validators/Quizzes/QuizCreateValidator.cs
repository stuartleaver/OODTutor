using FluentValidation;
using T802.Web.Models.Quiz;

namespace T802.Web.Validators.Quizzes
{
    public class QuizCreateValidator : BaseT802Validator<QuizCreateModel>
    {
        public QuizCreateValidator()
        {
            RuleFor(model => model.Name).NotEmpty().WithMessage("Name is Required");
            RuleFor(model => model.Description).NotEmpty().WithMessage("Description is Required");
        }
    }
}