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
        public ActionResult Category(int id, int? page)
        {
            int pageSize = 15;
            int pageIndex = 1;
            //IPagedList<Tbl_POST> post = null;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            Tbl_Tags tag = db.tagRepository.FindByID(id);
            if (tag != null)
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

        public ActionResult Search(SearchViewModel model,int? page)
        {
            int pageSize = 8;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<lstPostViewModel> post = new List<lstPostViewModel>().ToPagedList(pageIndex, pageSize);
            List<Tbl_Tags> taglist = new List<Tbl_Tags>();
            taglist.AddRange(model.post_tag.Where(m => m.Selected)
                .Select(m => new Tbl_Tags { TagID = int.Parse(m.Value), TagName = m.Text })
                );
            ViewBag.stitle = model.title;
            if ((model.Dynasty == null || model.Dynasty == Dynasty.Timeline) && taglist.Count == 0)
            {
                post = db.postRepository.AllPosts()
                    .Where(m => m.status && m.post_title.Contains(model.title))
                    .OrderBy(m => m.post_title.Contains(model.title))
                    .Select(m => new lstPostViewModel
                    {
                        post_id = m.post_id,
                        post_title = m.post_title,
                        post_teaser = m.post_teaser,
                        ViewCount = m.ViewCount,
                        AvatarImage = m.AvatarImage,
                        create_date = m.create_date
                    }
                    ).ToPagedList(pageIndex, pageSize);
                
            }
            else if(taglist.Count > 0)
            {

                using (DVCPContext conn = db.Context)
                {
                    post = (
                        // instance from context
                        from z in taglist
                        // join list tìm kiếm
                        join a in conn.Tbl_Tags on z.TagID equals a.TagID
                        // instance from navigation property
                        from b in a.Tbl_POST
                         //join to bring useful data
                        join c in conn.Tbl_POST on b.post_id equals c.post_id
                        where b.status == true
                        where b.dynasty == model.Dynasty.ToString()
                        // sắp theo ngày đăng mới nhất
                        orderby b.create_date descending
                        select new 
                        {
                            c.post_id,
                            c.post_title,
                            c.post_teaser,
                            c.ViewCount,
                            c.AvatarImage,
                            c.create_date
                        })
                        .Distinct().Select(c=> new lstPostViewModel
                        {
                            post_id = c.post_id,
                            post_title = c.post_title,
                            post_teaser = c.post_teaser,
                            ViewCount = c.ViewCount,
                            AvatarImage = c.AvatarImage,
                            create_date = c.create_date
                        })
                        .ToPagedList(pageIndex, pageSize);
                }

            }
            else
            {
                post = db.postRepository.AllPosts()
                    .Where(m => m.status && m.post_title.Contains(model.title))
                    .Where(m => m.dynasty == model.Dynasty.ToString())
                    .OrderBy(m => m.post_title.Contains(model.title))
                    .Select(m => new lstPostViewModel
                    {
                        post_id = m.post_id,
                        post_title = m.post_title,
                        post_teaser = m.post_teaser,
                        ViewCount = m.ViewCount,
                        AvatarImage = m.AvatarImage,
                        create_date = m.create_date
                    }
                    ).ToPagedList(pageIndex, pageSize);
                
            }
            
            return View(post);
        }
    }
}