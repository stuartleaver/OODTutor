using System.Web.Mvc;

namespace T802.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return View();

            if (User.IsInRole("GameMethod"))
            {
                return RedirectToAction("Index", "Game");
            }
            if (User.IsInRole("TraditionalMethod"))
            {
                return RedirectToAction("Index", "Traditional");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}