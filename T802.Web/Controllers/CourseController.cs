using System.Web.Mvc;
using T802.Services.Logging;
using T802.Web.Framework;

namespace T802.Web.Controllers
{
    [CustomAuthorize]
    public class CourseController : Controller
    {
        private readonly IStudentActivityService _studentActivityService;

        public CourseController(IStudentActivityService studentActivityService)
        {
            this._studentActivityService = studentActivityService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SingleResponsibilityPrinciple()
        {
            //activity log
            _studentActivityService.InsertActivity("Course.Material.SingleResponsibilityPrinciple", "ActivityLog.Course.Material.SingleResponsibilityPrinciple");

            return View();
        }

        public ActionResult OpenClosedPrinciple()
        {
            //activity log
            _studentActivityService.InsertActivity("Course.Material.OpenClosedPrinciple", "ActivityLog.Course.Material.OpenClosedPrinciple");

            return View();
        }

        public ActionResult LiskovSubstitutionPrinciple()
        {
            //activity log
            _studentActivityService.InsertActivity("Course.Material.LiskovSubstitutionPrinciple", "ActivityLog.Course.Material.LiskovSubstitutionPrinciple");

            return View();
        }

        public ActionResult InterfaceSegregationPrinciple()
        {
            //activity log
            _studentActivityService.InsertActivity("Course.Material.InterfaceSegregationPrinciple", "ActivityLog.Course.Material.InterfaceSegregationPrinciple");

            return View();
        }

        public ActionResult DependencyInversionPrinciple()
        {
            //activity log
            _studentActivityService.InsertActivity("Course.Material.DependencyInversionPrinciple", "ActivityLog.Course.Material.DependencyInversionPrinciple");

            return View();
        }

        public ActionResult PDF(string id)
        {
            var url = "";

            switch (id)
            {
                case "SingleResponsibilityPrinciple":
                    url = "https://docs.google.com/open?id=0ByOwmqah_nuGNHEtcU5OekdDMkk";
                    break;
                case "OpenClosedPrinciple":
                    url =
                        "http://docs.google.com/a/cleancoder.com/viewer?a=v&pid=explorer&chrome=true&srcid=0BwhCYaYDn8EgN2M5MTkwM2EtNWFkZC00ZTI3LWFjZTUtNTFhZGZiYmUzODc1&hl=en";
                    break;
                case "LiskovSubstitutionPrinciple":
                    url =
                        "http://docs.google.com/a/cleancoder.com/viewer?a=v&pid=explorer&chrome=true&srcid=0BwhCYaYDn8EgNzAzZjA5ZmItNjU3NS00MzQ5LTkwYjMtMDJhNDU5ZTM0MTlh&hl=en";
                    break;
                case "InterfaceSegregationPrinciple":
                    url =
                        "http://docs.google.com/a/cleancoder.com/viewer?a=v&pid=explorer&chrome=true&srcid=0BwhCYaYDn8EgOTViYjJhYzMtMzYxMC00MzFjLWJjMzYtOGJiMDc5N2JkYmJi&hl=en";
                    break;
                case "DependencyInversionPrinciple":
                    url =
                        "http://docs.google.com/a/cleancoder.com/viewer?a=v&pid=explorer&chrome=true&srcid=0BwhCYaYDn8EgMjdlMWIzNGUtZTQ0NC00ZjQ5LTkwYzQtZjRhMDRlNTQ3ZGMz&hl=en";
                    break;
                default:
                    url = "oodtutor.com";
                    break;
            }

            //activity log
            _studentActivityService.InsertActivity("Course.Material.PDF." + id, "ActivityLog.Course.Material.PDF." + id);

            return Redirect(url);
        }

        public ActionResult Video(string id)
        {
            var url = "";

            switch (id)
            {
                case "SingleResponsibilityPrinciple":
                    url = "http://www.dimecasts.net/Content/WatchEpisode/88";
                    break;
                case "OpenClosedPrinciple":
                    url =
                        "http://www.dimecasts.net/Content/WatchEpisode/90";
                    break;
                case "LiskovSubstitutionPrinciple":
                    url =
                        "http://www.dimecasts.net/Content/WatchEpisode/92";
                    break;
                case "InterfaceSegregationPrinciple":
                    url =
                        "http://www.dimecasts.net/Content/WatchEpisode/94";
                    break;
                case "DependencyInversionPrinciple":
                    url =
                        "http://www.dimecasts.net/Content/WatchEpisode/96";
                    break;
                default:
                    url = "oodtutor.com";
                    break;
            }

            //activity log
            _studentActivityService.InsertActivity("Course.Material.Video." + id, "ActivityLog.Course.Material.Video." + id);

            return Redirect(url);
        }
    }
}