using DVCP.CommonData;
using DVCP.Models;
using DVCP.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace DVCP.Controllers
{
    [Authorize(Roles = "admin,editor")]
    public class AdminController : Controller
    {
        UnitOfWork UnitOfWork = new UnitOfWork(new DVCPContext());
        //[Authorize(Roles ="admin,editor")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult newPost()
        {
            newPostViewModel model = new newPostViewModel
            {
                post_type = PostType.Normal,
                post_tag = PostData.getTagList(),
                Status = true,
                changeAvatar = true,
            };
            
            return View(model);
        }

        [HttpPost]
        public ActionResult newPost(newPostViewModel model)
        {
            var tag = string.Join(",", model.post_tag.Where(x => x.Selected == true).Select(i=>i.Value));
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(tag))
            {
                // Nếu như teaser trống hoặc quá ngắn, sẽ dùng content thay thế từ content bài viết
                if(model.post_teaser == null || model.post_teaser.Length <= 20 )
                {
                    model.post_teaser = CommonFunction.GetTeaserFromContent(model.post_content, 200);
                }
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                try
                {
                    if (model.avatarFile != null && model.avatarFile.ContentLength > 0)
                    {
                        string subPath = Server.MapPath("~/Upload/images/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }
                        string extension = Path.GetExtension(model.avatarFile.FileName);
                        model.AvatarImage = SlugGenerator.SlugGenerator.GenerateSlug(model.post_title) + extension;
                        model.avatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.AvatarImage);
                    }
                }
                catch (Exception ex)
                {
                    isSavedSuccessfully = false;
                }
                if(isSavedSuccessfully == true)
                {
                    tbl_User user = UnitOfWork.userRepository.FindByUsername(User.Identity.Name);
                    tbl_POST pOST = new tbl_POST
                    {
                        userid = user.userid,
                        dynasty = model.dynasty.ToString(),
                        create_date = DateTime.Now,
                        AvatarImage = model.AvatarImage,
                        post_content = model.post_content,
                        post_review = model.post_review,
                        post_tag = tag,
                        post_title = model.post_title,
                        post_type = (int)model.post_type,
                        ViewCount = 0,
                        Rated = (int)model.Rated,
                        post_teaser = model.post_teaser,
                        status = model.Status,
                    };
                    UnitOfWork.postRepository.AddPost(pOST);
                    UnitOfWork.Commit();
                    if (model.post_type.Equals(PostType.Slide))
                    {
                        return View("UploadImage", new uploadViewModel { id = pOST.post_id, title = pOST.post_title });
                    }
                    return RedirectToAction("ListPost");
                }
            }
            // nếu lỗi thì đẩy về tiếp
            //newPostViewModel nmodel = new newPostViewModel
            //{
            //    post_type = PostType.Normal,
            //    post_tag = PostData.getTagList(),
            //};

            return View(model);
        }
        [Authorize(Roles ="admin")]
        public ActionResult ListPost(string sortOrder, string CurrentSort, int? page, string titleStr)
        {
            //DVCPContext db = new DVCPContext();
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Title" : sortOrder;
            IPagedList<tbl_POST> post = null;
            ViewBag.Sort = "tăng dần";
            if (String.IsNullOrWhiteSpace(titleStr))
            {
                switch (sortOrder)
                {
                    case "Title":
                        ViewBag.sortname = "tiêu đề";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().OrderByDescending
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().OrderBy
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        } 
                        break;
                    case "CreateDate":
                        ViewBag.sortname = "ngày tạo";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().OrderByDescending
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }
                            
                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().OrderBy
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }
                            
                        break;
                    case "ViewCount":
                        ViewBag.sortname = "lượt xem";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().OrderByDescending
                                    (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                        }
                            
                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().OrderBy
                                    (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                        }
                            
                        break;
                    //default:
                    //    post = UnitOfWork.postRepository.AllPosts().ToPagedList(pageIndex, pageSize);
                    //    break;
                }
            }
            else
            {
                ViewBag.titleStr = titleStr;
                switch (sortOrder)
                {
                    case "Title":
                        ViewBag.sortname = "tiêu đề";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).OrderByDescending
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).OrderBy
                                      (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "CreateDate":
                        ViewBag.sortname = "ngày tạo";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).OrderByDescending
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).OrderBy
                                   (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "ViewCount":
                        ViewBag.sortname = "lượt xem";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).OrderByDescending
                                    (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).OrderBy
                                     (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    //default:
                    //    post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).ToPagedList(pageIndex, pageSize);
                    //    break;
                }
            }
            return View(post);
        }
        public ActionResult MyPost(string sortOrder, string CurrentSort, int? page,string titleStr)
        {
            //DVCPContext db = new DVCPContext();
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Title" : sortOrder;
            IPagedList<tbl_POST> post = null;
            ViewBag.Sort = "tăng dần";
            tbl_User user = UnitOfWork.userRepository.FindByUsername(User.Identity.Name);
            if (String.IsNullOrWhiteSpace(titleStr))
            {
                switch (sortOrder)
                {
                    case "Title":
                        ViewBag.sortname = "tiêu đề";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderByDescending
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderBy
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "CreateDate":
                        ViewBag.sortname = "ngày tạo";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderByDescending
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderBy
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }

                        break;
                    case "ViewCount":
                        ViewBag.sortname = "lượt xem";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderByDescending
                                    (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderBy
                                    (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                        }

                        break;
                        //default:
                        //    post = UnitOfWork.postRepository.AllPosts().ToPagedList(pageIndex, pageSize);
                        //    break;
                }
            }
            else
            {
                ViewBag.titleStr = titleStr;
                switch (sortOrder)
                {
                    case "Title":
                        ViewBag.sortname = "tiêu đề";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderByDescending
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderBy
                                      (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "CreateDate":
                        ViewBag.sortname = "ngày tạo";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderByDescending
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderBy
                                   (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "ViewCount":
                        ViewBag.sortname = "lượt xem";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderByDescending
                                    (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderBy
                                     (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                        //default:
                        //    post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).ToPagedList(pageIndex, pageSize);
                        //    break;
                }
            }
            return View(post);
        }
        public ActionResult UserPost(int id,string sortOrder, string CurrentSort, int? page, string titleStr)
        {
            //DVCPContext db = new DVCPContext();
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Title" : sortOrder;
            IPagedList<tbl_POST> post = null;
            ViewBag.Sort = "tăng dần";
            tbl_User user = UnitOfWork.userRepository.FindByID(id);
            if(user != null)
            {
                if (String.IsNullOrWhiteSpace(titleStr))
                {
                    switch (sortOrder)
                    {
                        case "Title":
                            ViewBag.sortname = "tiêu đề";
                            if (sortOrder.Equals(CurrentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderByDescending
                                        (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderBy
                                        (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                        case "CreateDate":
                            ViewBag.sortname = "ngày tạo";
                            if (sortOrder.Equals(CurrentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderByDescending
                                        (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderBy
                                        (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }

                            break;
                        case "ViewCount":
                            ViewBag.sortname = "lượt xem";
                            if (sortOrder.Equals(CurrentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderByDescending
                                        (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = UnitOfWork.postRepository.AllPosts().Where(u => u.userid == user.userid).OrderBy
                                        (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                            }

                            break;
                            //default:
                            //    post = UnitOfWork.postRepository.AllPosts().ToPagedList(pageIndex, pageSize);
                            //    break;
                    }
                }
                else
                {
                    ViewBag.titleStr = titleStr;
                    switch (sortOrder)
                    {
                        case "Title":
                            ViewBag.sortname = "tiêu đề";
                            if (sortOrder.Equals(CurrentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderByDescending
                                        (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }
                            else
                            {
                                post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderBy
                                          (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                        case "CreateDate":
                            ViewBag.sortname = "ngày tạo";
                            if (sortOrder.Equals(CurrentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderByDescending
                                        (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderBy
                                       (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                        case "ViewCount":
                            ViewBag.sortname = "lượt xem";
                            if (sortOrder.Equals(CurrentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderByDescending
                                        (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.userid == user.userid).OrderBy
                                         (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                            //default:
                            //    post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).ToPagedList(pageIndex, pageSize);
                            //    break;
                    }
                }
                return View(post);
            }
            else
            {
                return RedirectToAction("UserManager", "User");
            }
           
            
        }
        public ActionResult editPost(int id)
        {
            tbl_POST post = UnitOfWork.postRepository.FindByID(id);
            if(post == null)
            {
                return RedirectToAction("ListPost");
            }
            newPostViewModel model = new newPostViewModel();
            if (post.tbl_User.username == User.Identity.Name || User.IsInRole("admin") )
            {
                Dynasty dn;
                Enum.TryParse(post.dynasty, out dn);
                Rated rated;
                Enum.TryParse(post.Rated.ToString(), out rated);
                PostType type;
                Enum.TryParse(post.post_type.ToString(), out type);
                List<SelectListItem> tag = PostData.getTagList();
                string[] oldtag = post.post_tag.Split(',');
                foreach (var x in tag)
                {
                    foreach (var t in oldtag)
                    {
                        if (x.Value == t)
                        {
                            x.Selected = true;
                        }
                    }
                }
                 model = new newPostViewModel
                {
                    post_id = post.post_id,
                    dynasty = dn,
                    post_content = post.post_content,
                    AvatarImage = post.AvatarImage,
                    post_review = post.post_review,
                    changeAvatar = false,
                    imagepath = "/images/slides/" + post.post_id,
                    Rated = rated,
                    Status = post.status,
                    post_tag = tag,
                    post_teaser = post.post_teaser,
                    post_title = post.post_title,
                    post_type = type,
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult editPost(newPostViewModel model)
        {
            var tag = string.Join(",", model.post_tag.Where(x => x.Selected == true).Select(i => i.Value));
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(tag))
            {
                // Nếu như teaser trống hoặc quá ngắn, sẽ dùng content thay thế từ content bài viết
                if (model.post_teaser == null || model.post_teaser.Length <= 20)
                {
                    model.post_teaser = CommonFunction.GetTeaserFromContent(model.post_content, 200);
                }
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                
                try
                {
                    if (model.changeAvatar == true && model.avatarFile != null && model.avatarFile.ContentLength > 0)
                    {
                        string subPath = Server.MapPath("~/Upload/images/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }
                        //xóa ảnh cũ
                        bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + model.AvatarImage));
                        if(exist)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Upload/images/" + model.AvatarImage));
                        }
                        // lưu ảnh mới với tên là slug tiêu đề mới
                        string extension = Path.GetExtension(model.avatarFile.FileName);
                        model.AvatarImage = SlugGenerator.SlugGenerator.GenerateSlug(model.post_title) + extension;
                        model.avatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.AvatarImage);
                    }
                }
                catch (Exception ex)
                {
                    isSavedSuccessfully = false;
                }
                if(isSavedSuccessfully)
                {
                    tbl_POST pOST = UnitOfWork.postRepository.FindByID(model.post_id);
                    if (pOST.tbl_User.username != User.Identity.Name)
                    {
                        return RedirectToAction("ListPost");
                    }
                    pOST.dynasty = model.dynasty.ToString();
                    pOST.edit_date = DateTime.Now;
                    pOST.AvatarImage = model.AvatarImage;
                    pOST.post_content = model.post_content;
                    pOST.post_review = model.post_review;
                    pOST.post_tag = tag;
                    pOST.post_title = model.post_title;
                    pOST.post_type = (int)model.post_type;
                    pOST.Rated = (int)model.Rated;
                    pOST.post_teaser = model.post_teaser;
                    pOST.status = model.Status;
                    UnitOfWork.postRepository.UpdatePost(pOST);
                    UnitOfWork.Commit();
                    if (model.post_type.Equals(PostType.Slide))
                    {
                        return View("UploadImage", new uploadViewModel { id = pOST.post_id, title = pOST.post_title });
                    }

                }
                
                return RedirectToAction("ListPost");

            }
            newPostViewModel nmodel = new newPostViewModel
            {
                post_type = PostType.Normal,
                post_tag = PostData.getTagList(),
            };

            return View(nmodel);
        }
        //POST: tbl_POST/Delete/5
        [HttpPost]
        public JsonResult DeleteConfirmed(int id)
        {
            tbl_POST tbl_POST = UnitOfWork.postRepository.FindByID(id);
            string title = tbl_POST.post_title;
            if((tbl_POST.tbl_User.username == User.Identity.Name) || User.IsInRole("admin"))
            {
                UnitOfWork.postRepository.DeletePost(tbl_POST);
                if (tbl_POST.post_type == (int)PostType.Slide)
                {
                    string subPath = Server.MapPath("~/Files/images/slides/" + tbl_POST.post_id);
                    bool exists = System.IO.Directory.Exists(subPath);
                    if (exists)
                    {
                        System.IO.Directory.Delete(subPath, true);
                    }
                }
                //xóa ảnh đại diện cũ
                bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + tbl_POST.AvatarImage));
                if (exist)
                {
                    System.IO.File.Delete(Server.MapPath("~/Upload/images/" + tbl_POST.AvatarImage));
                }
                UnitOfWork.Commit();
                return Json(new { Message = "Xóa '" + title + "' thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = "Không thể xóa '" + title + "' <br /> vì đó không phải bài viết của bạn." },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult changeStatus(int id, bool state = false)
        {
            tbl_POST tbl_POST = UnitOfWork.postRepository.FindByID(id);
            string title = tbl_POST.post_title;
            tbl_POST.status = state;
            string prefix = state == true ? "Đăng" : "Hủy đăng";
            UnitOfWork.Commit();
            return Json(new { Message = prefix +" \"" + title + "\" thành công" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListSeries(string name,int? page)
        {
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Tbl_Series> sr = null;
            if (!String.IsNullOrWhiteSpace(name))
            {
                ViewBag.name = name;
                sr = UnitOfWork.seriesRepository.AllSeries().Where(m => m.seriesName.Contains(name)).OrderBy(x=>x.seriesID).ToPagedList(pageIndex,pageSize);
            }
            else
            {
                sr = UnitOfWork.seriesRepository.AllSeries().OrderBy(x => x.seriesID).ToPagedList(pageIndex, pageSize);
            }
            return View(sr);
        }
        /// <summary>
        /// Thêm series mới
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult addSerie(string name)
        {
            if(String.IsNullOrWhiteSpace(name))
            {
                return Json(new { Message = "Không được để trống tiêu đề series" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                UnitOfWork.seriesRepository.AddSeries(new Tbl_Series
                {
                    seriesName = name,
                });
                UnitOfWork.Commit();
                return Json(new { Message = "Tạo series" + " \"" + name + "\" thành công" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult editSerie(int id,string name)
        {
            if (string.IsNullOrWhiteSpace(name) || id == null)
            {
                return Json(new { Message = "Không được để trống tiêu đề series hoặc id" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                Tbl_Series series = UnitOfWork.seriesRepository.FindByID(id);
                series.seriesName = name;
                UnitOfWork.Commit();
                return Json(new { Message = "Edit series" + " \"" + series.seriesName + "\" thành công" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult DeleteSeries(int id)
        {
            Tbl_Series cate = UnitOfWork.seriesRepository.FindByID(id);
            string title = cate.seriesName;
            UnitOfWork.seriesRepository.Delete(cate);
            UnitOfWork.Commit();
            return Json(new { reload = true, Message = "Xóa '" + title + "' thành công" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Trình quản lý file
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        [HttpGet]
       public ActionResult FileManager(string subFolder)
        {
            FileViewModel model = new FileViewModel { Folder = "Files", SubFolder = subFolder };
            return View(model);
        }
        
        [HttpPost]
        public ActionResult UploadImage(uploadViewModel model)
        {
            if(ModelState.IsValid)
            {
                bool isSavedSuccessfully = true;
                try
                {
                    /////throw new Exception();
                    foreach (string fileName in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[fileName];
                        //Save file content goes here
                        if (file != null && file.ContentLength > 0)
                        {
                            string subPath = Server.MapPath("~/Upload/Files/Images/slides/" + model.id);
                            bool exists = System.IO.Directory.Exists(subPath);
                            if (!exists)
                            {
                                System.IO.Directory.CreateDirectory(subPath);
                            }
                            file.SaveAs(Server.MapPath("~/Upload/Files/Images/slides/" + model.id + "/") + file.FileName);
                            //return Json(new { Message = "Tải file "+file.FileName+" lên thành công." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully == false)
                {
                    Response.StatusCode = 500;
                    return Json(new { Message = "Tải file lên bị lỗi." }, JsonRequestBehavior.AllowGet);
                    //return Json(new {success= false, Message = "Error in saving file" });
                }
                else
                {
                    Response.StatusCode = 200;
                    return Json(new { Message = "Hoàn tất upload ảnh.", Url = "/Admin/ListPost" }, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ListPost");
                }
                   
            }
            return View();
        }
        [Authorize(Roles ="admin")]
        public ActionResult InfoChange()
        {
            info info = UnitOfWork.infoRepository.FindByID();
            infoViewModel model = new infoViewModel
            {
                web_name = info.web_name,
                web_about = info.web_about,
                web_des = info.web_des
            };
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult InfoChange(infoViewModel model)
        {
            if(ModelState.IsValid)
            {
                info info = UnitOfWork.infoRepository.FindByID();
                info.web_des = model.web_des;
                info.web_name = model.web_name;
                info.web_about = model.web_about;
                UnitOfWork.Commit();
            }
            return View();
        }
    }
}