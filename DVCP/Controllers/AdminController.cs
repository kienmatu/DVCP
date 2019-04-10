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
            };
            
            return View(model);
        }

        [HttpPost]
        public ActionResult newPost(newPostViewModel model)
        {
            var tag = string.Join(",", model.post_tag.Where(x => x.Selected == true).Select(i=>i.Value));
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(tag))
            {
                tbl_User user = UnitOfWork.userRepository.FindByUsername(User.Identity.Name);
                tbl_POST pOST = new tbl_POST
                {
                    userid = user.userid,
                    dynasty = model.dynasty.ToString(),
                    create_date = DateTime.Now,
                    //AvatarImage = null,
                    post_content = model.post_content,
                    post_review = model.post_review,
                    post_tag = tag,
                    post_title = model.post_title,
                    post_type = (int)model.post_type,
                    ViewCount = 0,
                    Rated = (int)model.Rated,
                    post_teaser = model.post_teaser,
                };
                UnitOfWork.postRepository.AddPost(pOST);
                UnitOfWork.Commit();
                if(model.post_type.Equals(PostType.Slide))
                {
                    return View("UploadImage", new uploadViewModel { id = pOST.post_id,title = pOST.post_title });
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
        public ActionResult ListPost(string sortOrder, string CurrentSort, int? page)
        {
            DVCPContext db = new DVCPContext();
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Title" : sortOrder;
            IPagedList<tbl_POST> post = null;
            switch (sortOrder)
            {
                case "Title":
                    if (sortOrder.Equals(CurrentSort))
                        post = db.tbl_POST.OrderByDescending
                                (m => m.post_title).ToPagedList(pageIndex, pageSize);
                    else
                        post = db.tbl_POST.OrderBy
                                (m => m.post_title).ToPagedList(pageIndex, pageSize);
                    break;
                case "CreateDate":
                    if (sortOrder.Equals(CurrentSort))
                        post = db.tbl_POST.OrderByDescending
                                (m => m.create_date).ToPagedList(pageIndex, pageSize);
                    else
                        post = db.tbl_POST.OrderBy
                                (m => m.create_date).ToPagedList(pageIndex, pageSize);
                    break;
                case "ViewCount":
                    if (sortOrder.Equals(CurrentSort))
                        post = db.tbl_POST.OrderByDescending
                                (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                    else
                        post = db.tbl_POST.OrderBy
                                (m => m.ViewCount).ToPagedList(pageIndex, pageSize);
                    break;
                    //case "CreateDate":
                    //    employees = db.Employees.OrderBy
                    //        (m => m.Name).ToPagedList(pageIndex, pageSize);
                    //    break;
            }
            return View(post);
        }

        public ActionResult Delete(int id)
        {
            
            tbl_POST tbl_POST = UnitOfWork.postRepository.FindByID(id);
            if (tbl_POST == null)
            {
                return HttpNotFound();
            }
            return View(tbl_POST);
        }

        // POST: tbl_POST/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_POST tbl_POST = UnitOfWork.postRepository.FindByID(id);
            UnitOfWork.postRepository.DeletePost(tbl_POST);
            UnitOfWork.Commit();
            return RedirectToAction("Index");
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
                            string subPath = Server.MapPath("~/Files/images/slides/" + model.id);
                            bool exists = System.IO.Directory.Exists(subPath);
                            if (!exists)
                            {
                                System.IO.Directory.CreateDirectory(subPath);
                            }
                            file.SaveAs(Server.MapPath("~/Files/images/slides/" + model.id + "/") + file.FileName);
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
    }
}