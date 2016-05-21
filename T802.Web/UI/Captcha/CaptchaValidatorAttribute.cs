using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using T802.Web.Models.Student;
using T802.Core;

namespace T802.Web.UI.Captcha
{
    public class CaptchaValidatorAttribute : ActionFilterAttribute
    {
        public string RecaptchaResponse { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var recaptchaResponse = filterContext.HttpContext.Request["g-recaptcha-response"];

            //secret that was generated in key value pair
            string secret = AppSettings.Get<string>("CaptchaServer");

            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret,
                        recaptchaResponse));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            //this will push the result value into a parameter in our Action  
            filterContext.ActionParameters["captchaValid"] = captchaResponse.Success;

            base.OnActionExecuting(filterContext);
        }
    }
}