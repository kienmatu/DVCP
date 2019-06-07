using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVCP.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork db = new UnitOfWork(new DVCPContext());
        public ActionResult Index()
        {
            ViewBag.Title = db.infoRepository.FindByID(1).web_name;
            return View(db.postRepository.AllPosts().OrderByDescending(m=>m.create_date).Take(10).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ViewResult _HotPost()
        {
            return View();
        }
        

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ViewPost(int id)
        {
            Tbl_POST p = db.postRepository.FindByID(id);
            if (p != null)
            {
                p.ViewCount++;
                db.Commit();
                return View(p);
            }
                
            return RedirectToAction("Index");

        }
    }
}