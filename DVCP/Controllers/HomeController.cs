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
            return View(db.postRepository.AllPosts().Where(m=>m.status==true)
                .OrderByDescending(m=>m.create_date).Take(7).ToList());
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
        public ActionResult ViewPost(string title)
        {
            if(!String.IsNullOrWhiteSpace(title))
            {
                Tbl_POST p = db.postRepository.FindBySlug(title);
                if (p != null)
                {
                    p.ViewCount++;
                    db.Commit();
                    List<TagList> tagLists = p.Tbl_Tags.Select(m => new TagList
                    {
                        id = m.TagID,
                        name = m.TagName,
                        slug = SlugGenerator.SlugGenerator.GenerateSlug(m.TagName) + "-" + m.TagID
                    }).ToList();
                    return View(new ViewPostViewModel
                    {
                        post_id = p.post_id,
                        dynasty = p.dynasty,
                        create_date = p.create_date,
                        //firstTag = tagLists.FirstOrDefault().name,
                        post_review = p.post_review,
                        post_tag = p.post_tag,
                        AvatarImage = p.AvatarImage,
                        edit_date = p.edit_date,
                        post_content = p.post_content,
                        post_teaser = p.post_teaser,
                        post_title = p.post_title,
                        post_type = p.post_type,
                        Rated = p.Rated,
                        userid = p.userid,
                        ViewCount = p.ViewCount,
                        tagLists = tagLists,

                    });
                }
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
        public ActionResult Category(int? id, int? page)
        {
            if(id!=null)
            {
                int pageSize = 15;
                int pageIndex = 1;
                //IPagedList<Tbl_POST> post = null;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                Tbl_Tags tag = db.tagRepository.FindByID(id.Value);
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

            return View("CategoryAll");

        }
        public ActionResult Dynasty(int? dynasty, int? page)
        {
            if(dynasty != null)
            {
                int pageSize = 10;
                int pageIndex = 1;
                IPagedList<lstPostViewModel> post = null;
                Dynasty d = (Dynasty)dynasty;
                ViewBag.catname = d.GetDisplayName();
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                post = db.postRepository.AllPosts()
                    .Where(m => m.status)
                    .Where(m => m.dynasty.Equals(d.ToString()))
                    .OrderByDescending(m => m.create_date)
                    .Select(c => new lstPostViewModel
                    {
                        post_id = c.post_id,
                        post_title = c.post_title,
                        post_teaser = c.post_teaser,
                        ViewCount = c.ViewCount,
                        AvatarImage = c.AvatarImage,
                        create_date = c.create_date,
                        tagsname = c.Tbl_Tags.FirstOrDefault().TagName,
                        slug = c.post_slug
                    }).ToPagedList(pageIndex, pageSize);

                return View(post);
            }
            return View("DynastyAll");
            

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
            bool title = String.IsNullOrWhiteSpace(model.title);
            bool tag = taglist.Count == 0;
            bool dynasty = model.Dynasty == null;
            var check = 0;
            if(title && tag &&  dynasty)
            {
                // cả 3 cái đều null
                check = 1;
            }
            else if (!title && !tag && !dynasty)
            {
                // cả 3 cái đều ko null
                check = 2;
            }
            else if(!title && tag && dynasty)
            {
                // chỉ title
                check = 3;
            }
            else if(title && !tag && dynasty)
            {
                // chỉ tag
                check = 4;
            }
            else if (title && tag && !dynasty)
            {
                // chỉ DN
                check = 5;
            }
            else if (!title && !tag && dynasty)
            {
                // title và tag
                check = 6;
            }
            else if (!title && tag && !dynasty)
            {
                // title và dn
                check = 7;
            }
            else if (title && !tag && !dynasty)
            {
                // tag và dn
                check = 8;
            }
            switch (check)
            {
                default:
                case 1:
                    IQueryable<Tbl_POST> x = db.postRepository.AllPosts()
                    .Where(m => m.status)
                    .OrderByDescending(m => m.create_date);
                    post =
                    x.Select(m => new lstPostViewModel
                    {
                        post_id = m.post_id,
                        post_title = m.post_title,
                        post_teaser = m.post_teaser,
                        ViewCount = m.ViewCount,
                        AvatarImage = m.AvatarImage,
                        create_date = m.create_date,
                        tagsname = m.Tbl_Tags.FirstOrDefault().TagName,
                        slug = m.post_slug
                    }
                    ).ToPagedList(pageIndex, pageSize);
                    break;
                case 2:
                    using (DVCPContext conn = db.Context)
                    {
                        var query = (
                            // instance from context
                            from z in taglist
                            // join list tìm kiếm
                            join a in conn.Tbl_Tags on z.TagID equals a.TagID
                            // instance from navigation property
                            from b in a.Tbl_POST
                            // join to bring useful data
                            join c in conn.Tbl_POST on b.post_id equals c.post_id
                            where c.status == true
                            where c.dynasty == model.Dynasty.ToString()
                            where c.post_title.ToLower().Contains(model.title.ToLower())
                            // sắp theo 
                            orderby c.Rated
                            select new
                            {
                                c.post_id,
                                c.post_title,
                                c.post_teaser,
                                c.ViewCount,
                                c.AvatarImage,
                                c.create_date,
                                c.Tbl_Tags.FirstOrDefault().TagName,
                                c.post_slug

                            }).Distinct();
                        //DISTINCT ĐỂ SAU KHI SELECT ĐỐI TƯỢNG MỚI ĐƯỢC
                        //VÌ THẰNG DƯỚI KHÔNG EQUAL HASHCODE
                        post = query.Select(c => new lstPostViewModel
                        {
                            post_id = c.post_id,
                            post_title = c.post_title,
                            post_teaser = c.post_teaser,
                            ViewCount = c.ViewCount,
                            AvatarImage = c.AvatarImage,
                            create_date = c.create_date,
                            tagsname = c.TagName,
                            slug = c.post_slug
                        }).ToPagedList(pageIndex, pageSize);

                    }
                    break;
                case 3:
                    var p = db.postRepository.AllPosts()
                    .Where(m => m.status)
                    .Where(m => m.post_title.Contains(model.title))
                    .OrderBy(m => m.post_title.Contains(model.title));
                    post =
                    p.Select(m => new lstPostViewModel
                    {
                        post_id = m.post_id,
                        post_title = m.post_title,
                        post_teaser = m.post_teaser,
                        ViewCount = m.ViewCount,
                        AvatarImage = m.AvatarImage,
                        create_date = m.create_date,
                        tagsname = m.Tbl_Tags.FirstOrDefault().TagName,
                        slug = m.post_slug
                    }
                    ).ToPagedList(pageIndex, pageSize);
                    break;
                case 4:
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
                            where c.status == true
                            // sắp theo ngày đăng mới nhất
                            orderby b.create_date descending
                            select new
                            {
                                c.post_id,
                                c.post_title,
                                c.post_teaser,
                                c.ViewCount,
                                c.AvatarImage,
                                c.create_date,
                                c.Tbl_Tags.FirstOrDefault().TagName,
                                c.post_slug
                            })
                            //DISTINCT ĐỂ SAU KHI SELECT ĐỐI TƯỢNG MỚI ĐƯỢC
                            //VÌ THẰNG DƯỚI KHÔNG EQUAL HASHCODE
                            .Distinct().Select(c => new lstPostViewModel
                            {
                                post_id = c.post_id,
                                post_title = c.post_title,
                                post_teaser = c.post_teaser,
                                ViewCount = c.ViewCount,
                                AvatarImage = c.AvatarImage,
                                create_date = c.create_date,
                                tagsname = c.TagName,
                                slug = c.post_slug
                                
                            })
                            .ToPagedList(pageIndex, pageSize);
                    }
                    break;
                case 5:
                    post = db.postRepository.AllPosts()
                 .Where(m => m.status)
                 .Where(m => m.dynasty.Equals(model.Dynasty.ToString()))
                 .OrderByDescending(m => m.create_date)
                 .Select(m => new lstPostViewModel
                 {
                     post_id = m.post_id,
                     post_title = m.post_title,
                     post_teaser = m.post_teaser,
                     ViewCount = m.ViewCount,
                     AvatarImage = m.AvatarImage,
                     create_date = m.create_date,
                     tagsname = m.Tbl_Tags.FirstOrDefault().TagName,
                     slug = m.post_slug
                 }
                 ).ToPagedList(pageIndex, pageSize);
                    break;
                case 6:
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
                            where c.post_title.ToLower().Contains(model.title.ToLower())
                            where c.status == true
                            // sắp theo so khớp
                            orderby c.post_title.Contains(model.title)
                            select new
                            {
                                c.post_id,
                                c.post_title,
                                c.post_teaser,
                                c.ViewCount,
                                c.AvatarImage,
                                c.create_date,
                                c.Tbl_Tags.FirstOrDefault().TagName,
                                c.post_slug
                            })
                            //DISTINCT ĐỂ SAU KHI SELECT ĐỐI TƯỢNG MỚI ĐƯỢC
                            //VÌ THẰNG DƯỚI KHÔNG EQUAL HASHCODE
                            .Distinct().Select(c => new lstPostViewModel
                            {
                                post_id = c.post_id,
                                post_title = c.post_title,
                                post_teaser = c.post_teaser,
                                ViewCount = c.ViewCount,
                                AvatarImage = c.AvatarImage,
                                create_date = c.create_date,
                                tagsname = c.TagName,
                                slug = c.post_slug

                            })
                            .ToPagedList(pageIndex, pageSize);
                    }
                    break;
                case 7:
                    post = db.postRepository.AllPosts()
                 .Where(m => m.status)
                 .Where(m => m.post_title.Contains(model.title))
                 .Where(m => m.dynasty.Equals(model.Dynasty.ToString()))
                 .OrderBy(m => m.post_title.Contains(model.title))
                 .Select(m => new lstPostViewModel
                 {
                     post_id = m.post_id,
                     post_title = m.post_title,
                     post_teaser = m.post_teaser,
                     ViewCount = m.ViewCount,
                     AvatarImage = m.AvatarImage,
                     create_date = m.create_date,
                    tagsname = m.Tbl_Tags.FirstOrDefault().TagName,
                    slug = m.post_slug
                 }
                 ).ToPagedList(pageIndex, pageSize);
                    break;
                case 8:
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
                            where c.dynasty == model.Dynasty.ToString()
                            where c.status == true
                            // sắp theo so khớp
                            orderby c.create_date descending
                            select new
                            {
                                c.post_id,
                                c.post_title,
                                c.post_teaser,
                                c.ViewCount,
                                c.AvatarImage,
                                c.create_date,
                                c.Tbl_Tags.FirstOrDefault().TagName,
                                c.post_slug
                            })
                            //DISTINCT ĐỂ SAU KHI SELECT ĐỐI TƯỢNG MỚI ĐƯỢC
                            //VÌ THẰNG DƯỚI KHÔNG EQUAL HASHCODE
                            .Distinct().Select(c => new lstPostViewModel
                            {
                                post_id = c.post_id,
                                post_title = c.post_title,
                                post_teaser = c.post_teaser,
                                ViewCount = c.ViewCount,
                                AvatarImage = c.AvatarImage,
                                create_date = c.create_date,
                                tagsname = c.TagName,
                                slug = c.post_slug
                            })
                            .ToPagedList(pageIndex, pageSize);
                    }
                    break;

            }
            
            return View(post);
        }
    }
}