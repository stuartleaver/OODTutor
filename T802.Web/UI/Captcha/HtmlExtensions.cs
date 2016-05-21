using System.Web.Mvc;
using T802.Core;

namespace T802.Web.UI.Captcha
{
    public static class HtmlExtensions
    {
        public static string GenerateCaptcha(this HtmlHelper helper)
        {
            var captcha = new TagBuilder("div");
            captcha.AddCssClass("g-recaptcha");
            captcha.Attributes.Add("data-sitekey", AppSettings.Get<string>("CaptchaClient"));

            return captcha + "<br /";
        }
    }
}