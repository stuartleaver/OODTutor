using FluentValidation;
using T802.Web.Models.Install;

namespace T802.Web.Validators.Install
{
    public class InstallValidator : BaseT802Validator<InstallModel>
    {
        public InstallValidator()
        {
            RuleFor(model => model.AdminUsername).NotEmpty().WithMessage("Admin AdminUsername Required");
            RuleFor(x => x.AdminPassword).NotEmpty().WithMessage("Admin Password Required");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password Required");
            RuleFor(x => x.AdminPassword).Equal(x => x.ConfirmPassword).WithMessage("Passwords Do Not Match");
        }
    }
}