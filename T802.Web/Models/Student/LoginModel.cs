using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace T802.Web.Models.Student
{
    public class LoginModel : BaseT802Model
    {
        [DisplayName("Email")]
        [AllowHtml]
        public string Email { get; set; }

        [DisplayName("User Name")]
        [AllowHtml]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [AllowHtml]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}