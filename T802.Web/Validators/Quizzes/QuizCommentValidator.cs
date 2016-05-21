using FluentValidation;
using T802.Web.Models.Quiz;

namespace T802.Web.Validators.Quizzes
{
    public class QuizCommentValidator : BaseT802Validator<QuizCommentModel>
    {
        public QuizCommentValidator()
        {
            RuleFor(model => model.Comment).NotEmpty().WithMessage("Comment is Required");
        }
    }
}