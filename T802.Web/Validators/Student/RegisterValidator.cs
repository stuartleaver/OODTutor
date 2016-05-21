using System;
using FluentValidation;
using T802.Core;
using T802.Web.Models.Student;

namespace T802.Web.Validators.Student
{
    public class RegisterValidator : BaseT802Validator<RegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Required");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Required");
            RuleFor(x => x.Password).Length(AppSettings.Get<int>("MinimumStudentPasswordLength"), 999).WithMessage(string.Format("Length Validation"), AppSettings.Get<int>("MinimumStudentPasswordLength"));
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Required");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Entered Passwords Do Not Match");
        }
    }
}