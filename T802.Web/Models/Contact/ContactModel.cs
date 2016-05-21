using FluentValidation.Attributes;
using T802.Web.Validators.Contact;

namespace T802.Web.Models.Contact
{
    [Validator(typeof(ContactValidator))]
    public class ContactModel : BaseT802Model
    {
        public string Username { get; set; }
        public string Message { get; set; }
    }
}