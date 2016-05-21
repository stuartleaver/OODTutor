using FluentValidation;
using T802.Web.Models.Contact;

namespace T802.Web.Validators.Contact
{
    public class ContactValidator : BaseT802Validator<ContactModel>
    {
        public ContactValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is Required");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message is Required");
        }
    }
}