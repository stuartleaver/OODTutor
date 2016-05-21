using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using SendGrid;
using T802.Web.Models.Contact;
using T802.Services.Logging;

namespace T802.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IStudentActivityService _studentActivityService;

        public ContactController(IStudentActivityService studentActivityService)
        {
            _studentActivityService = studentActivityService;            
        }

        public ActionResult Index()
        {
            var model = new ContactModel();

            //activity log
            _studentActivityService.InsertActivity("Contact.Form", "ActivityLog.Contact.Form");

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ContactModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            SendEmail("stuartleaveruk@gmail.com", model);

            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        public void SendEmail(string sendTo, ContactModel model)
        {
            // Create the email object first, then add the properties.
            SendGridMessage message = new SendGridMessage();
            message.AddTo(sendTo);
            message.From = new MailAddress("info@oodtutor.com", "OOD Tutor");
            message.Subject = "OOD Tutor Message";
            message.Html = "<p>Username: " + model.Username + "</p><p>Message: " + model.Message + "</p>";

            Send(message);
        }

        private void Send(SendGridMessage message)
	    {
            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential("azure_02f88951b0279955795b1a75624fb046@azure.com", "Lcf8K0NF1gJo7Fd");

            // Create a Web transport for sending email.
            var transportWeb = new SendGrid.Web(credentials);

            // Send the email.
            try
            {
                transportWeb.Deliver(message);

                //activity log
                _studentActivityService.InsertActivity("Contact.Form.Sent", "ActivityLog.Contact.Form.Sent");
            }
            catch (Exception ex)
            {
                // This is bad
            }
	    }
	}
}