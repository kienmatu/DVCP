using DVCP.Models;
using DVCP.ViewModel;
using PagedList;
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
            return View(db.postRepository.AllPosts().Where(m=>m.status==true).OrderByDescending(m=>m.create_date).Take(10).ToList());
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
        public ActionResult Category(int id,int? page)
        {
            int pageSize = 15;
            int pageIndex = 1;
            IPagedList<Tbl_POST> post = null;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            Tbl_Tags tag = db.tagRepository.FindByID(id);
            if(tag!=null)
            {
                using (DVCPContext conn = db.Context)
                {
                    var result = (
                        // instance from context
                        from a in conn.Tbl_Tags
                            // instance from navigation property
                    from b in a.Tbl_POST
                        //join to bring useful data
                    join c in conn.Tbl_POST on b.post_id equals c.post_id
                        where a.TagID == id && b.status == true
                        orderby b.create_date descending
                        select new lstPostViewModel
                        {
                            post_id = c.post_id,
                            post_title = c.post_title,
                            post_teaser = c.post_teaser,
                            ViewCount = c.ViewCount,
                            AvatarImage = c.AvatarImage,
                            create_date = c.create_date
                        }).ToPagedList(pageIndex, pageSize);
                    ViewBag.catname = tag.TagName;
                    return View(result);
                }
            }
            return HttpNotFound();
            
        }
    }
}