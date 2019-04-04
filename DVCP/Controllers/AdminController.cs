using DVCP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVCP.Controllers
{
    [Authorize(Roles = "admin,editor")]
    public class AdminController : Controller
    {
        // GET: Admin
        //[Authorize(Roles ="admin,editor")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult newPost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult newPost(newPostViewModel model)
        {
            return View();
        }
        public ActionResult ListPost()
        {
            return View();
        }
    }
}