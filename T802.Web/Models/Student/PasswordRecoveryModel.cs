using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using T802.Web.Validators.Student;

namespace T802.Web.Models.Student
{
    [Validator(typeof(PasswordRecoveryValidator))]
    public class PasswordRecoveryModel : BaseT802Model
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Result { get; set; }
        public bool SuccessfullyChanged { get; set; }
    }
}