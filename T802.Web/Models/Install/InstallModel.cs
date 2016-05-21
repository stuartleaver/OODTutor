using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using T802.Web.Validators.Install;

namespace T802.Web.Models.Install
{
    [Validator(typeof(InstallValidator))]
    public partial class InstallModel : BaseT802Model
    {
        [AllowHtml]
        public string AdminUsername { get; set; }
        [AllowHtml]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }
        [AllowHtml]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}