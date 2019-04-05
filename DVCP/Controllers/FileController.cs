using ElFinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVCP.Controllers
{
    public class FileController : Controller
    {
        public virtual ActionResult Index(string folder, string subFolder)
        {
            var driver = new FileSystemDriver();

            var root = new Root(new DirectoryInfo(Server.MapPath("~/Upload/" + folder)),
                "http://" + Request.Url.Authority + "/Upload/" + folder + "/")
            {
                IsReadOnly = false,
                Alias = "Root",
                MaxUploadSizeInMb = 100
            };

            if (!string.IsNullOrEmpty(subFolder))
            {
                root.StartPath = new DirectoryInfo(Server.MapPath("~/Upload/" + folder + "/" + subFolder));
            }

            driver.AddRoot(root);
            var connector = new Connector(driver);
            return connector.Process(HttpContext.Request);
        }

        public virtual ActionResult SelectFile(string target)
        {
            var driver = new FileSystemDriver();

            driver.AddRoot(
                new Root(
                    new DirectoryInfo(Server.MapPath("~/Upload")),
                    "http://" + Request.Url.Authority + "/Upload")
                { IsReadOnly = false });

            var connector = new Connector(driver);

            return Json(connector.GetFileByHash(target).FullName);
        }
    }

}