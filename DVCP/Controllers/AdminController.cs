using DVCP.CommonData;
using DVCP.Models;
using DVCP.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                post_tag = PostData.getDynastyList(),
            };
            
            return View(model);
        }

        [HttpPost]
        public ActionResult newPost(newPostViewModel model)
        {
            if(ModelState.IsValid)
            {
                var tag = string.Join(",", model.post_tag);
                tbl_User user = UnitOfWork.userRepository.FindByUsername(User.Identity.Name);
                UnitOfWork.postRepository.AddPost(
                    new tbl_POST
                    {
                        userid = user.userid,
                        dynasty = model.dynasty.ToString(),
                        create_date = DateTime.Now,
                        AvatarImage = "",
                        post_content = model.post_content,
                        post_review = model.post_review,
                        post_tag = tag,
                        post_title = model.post_title,
                        post_type = (int)model.post_type,
                        ViewCount = 0,
                        Rated = (int)model.Rated,
                        post_teaser = model.post_teaser,
                    }
                    );
            }
            return View();
        }
        public ActionResult ListPost()
        {
            return View();
        }
        [HttpGet]
       public ActionResult FileManager(string subFolder)
        {
            DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Files/"));
            // Enumerating all 1st level directories of a given root folder (MyFolder in this case) and retrieving the folders names.
            var folders = di.GetDirectories().ToList().Select(d => d.Name);

            FileViewModel model = new FileViewModel(){ Folder = "images", SubFolder = subFolder };

            return View(model);
        }
    }
}