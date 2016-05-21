using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using T802.Web.Validators.Student;

namespace T802.Web.Models.Student
{
    [Validator(typeof(RegisterValidator))]
    public partial class RegisterModel : BaseT802Model
    {
        [DisplayName("Email")]
        [AllowHtml]
        public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }
        [DisplayName("Username")]
        [AllowHtml]
        public string Username { get; set; }

        public bool CheckUsernameAvailabilityEnabled { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [AllowHtml]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [AllowHtml]
        public string ConfirmPassword { get; set; }
    }
}