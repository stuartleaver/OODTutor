using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using T802.Services.Logging;
using T802.Services.Students;
using T802.Web.Framework;
using T802.Web.Models.Download;

namespace T802.Web.Controllers
{
    [CustomAuthorize]
    public class DownloadController : Controller
    {
        private readonly IStudentActivityService _studentActivityService;
        private readonly List<FileModel> Files;

        public DownloadController(IStudentActivityService studentActivityService)
        {
            this._studentActivityService = studentActivityService;

            Files = new List<FileModel>
            {
                new FileModel
                {
                    FileId = "Start",
                    FileUrl = "https://s3-eu-west-1.amazonaws.com/oodtutor/OODTutor-On-SOLID.zip"
                },
                new FileModel
                {
                    FileId = "SRP",
                    FileUrl = "https://s3-eu-west-1.amazonaws.com/oodtutor/OODTutor-On-SRP.zip"
                },
                new FileModel
                {
                    FileId = "OCP",
                    FileUrl = "https://s3-eu-west-1.amazonaws.com/oodtutor/OODTutor-On-OCP.zip"
                },
                new FileModel
                {
                    FileId = "LSP",
                    FileUrl = "https://s3-eu-west-1.amazonaws.com/oodtutor/OODTutor-On-LSP.zip"
                },
                new FileModel
                {
                    FileId = "ISP",
                    FileUrl = "https://s3-eu-west-1.amazonaws.com/oodtutor/OODTutor-On-ISP.zip"
                },
                new FileModel
                {
                    FileId = "DIP",
                    FileUrl = "https://s3-eu-west-1.amazonaws.com/oodtutor/OODTutor-On-DIP.zip"
                }
            };
        }

        public FileResult File(string id)
        {
            var currentFileName = (from fls in Files
                                   where fls.FileId == id
                                   select fls.FileUrl).First();

            if (String.IsNullOrEmpty(currentFileName)) return null;
            
            using (WebClient wc = new WebClient())
            {
                var byteArr = wc.DownloadData(currentFileName);

                //activity log
                _studentActivityService.InsertActivity("Course.Material.SampleCode", "ActivityLog.Course.Material.SampleCode." + id);

                return File(byteArr, "application/x-zip-compressed");
            }
        }
    }
}