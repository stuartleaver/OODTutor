using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using T802.Web.Models.Student;
using FluentValidation;
using T802.Core;

namespace T802.Web.Validators.Student
{
    public class PasswordRecoveryValidator : BaseT802Validator<PasswordRecoveryModel>
    {
        public PasswordRecoveryValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username Required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Required");
            RuleFor(x => x.Password).Length(AppSettings.Get<int>("MinimumStudentPasswordLength"), 999).WithMessage(string.Format("Length Validation"), AppSettings.Get<int>("MinimumStudentPasswordLength"));
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Required");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Entered Passwords Do Not Match");
        }
    }
}