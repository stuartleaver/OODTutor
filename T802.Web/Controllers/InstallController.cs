using System;
using System.Web.Mvc;
using T802.Services.Installation;
using T802.Web.Models.Install;

namespace T802.Web.Controllers
{
    public class InstallController : Controller
    {
        private readonly IInstallationService _installationService;

        public InstallController(IInstallationService installationService)
        {
            _installationService = installationService;
        }
        //
        // GET: /Install/
        public ActionResult Index()
        {
            if (_installationService.IsDatabaseInstalled())
                return RedirectToAction("Index", "Home");

            var model = new InstallModel()
            {
                AdminUsername = ""
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(InstallModel model)
        {
            if (!ModelState.IsValid) 
                return View();

            try
            {
                _installationService.InstallData(model.AdminUsername, model.AdminPassword);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", string.Format("Setup Failed {0}", exception.Message));
            }

            return View();
        }
    }
}