using FluentValidation;
using T802.Web.Models.Student;

namespace T802.Web.Validators.Student
{
    public class LoginValidator : BaseT802Validator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username Required");
            RuleFor(x => x.Username).EmailAddress().WithMessage("Wrong Username");
        }
    }
}