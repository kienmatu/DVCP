using DVCP.Models;
using DVCP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DVCP.Controllers
{
    public class UserController : Controller
    {
        UnitOfWork UnitOfWork = new UnitOfWork(new DVCPContext());
        // GET: User
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "admin")]
        public ActionResult UserManager()
        {
           List<User> lstUser = UnitOfWork.userRepository.AllUsers().ToList();
           return View(lstUser);
        }
        public void setCookie(string username, bool rememberme = false, string role = "normal")
        {
            var authTicket = new FormsAuthenticationTicket(
                               1,
                               username,
                               DateTime.Now,
                               DateTime.Now.AddMinutes(120),
                               rememberme,
                               role
                               );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {

            if (ModelState.IsValid)
            {
                User user = UnitOfWork.userRepository.FindByUsername(model.Username);
                if (user != null)
                {
                    if (user.password == CommonData.CommonFunction.CalculateMD5Hash(model.Password) && user.status == true)
                    {
                        setCookie(user.username, model.RememberMe, user.userrole);
                        if (ReturnUrl != null)
                            return Redirect(ReturnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
                    return View();

                }
            }
            
            ViewBag.Error = "Wrong username or password!";
            return View();
        }
        [Authorize(Roles="admin")]
        public ActionResult createUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult createUser(userListViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = UnitOfWork.userRepository.FindByUsername(model.username);
                if(user == null)
                {
                    User nuser = new User
                    {
                        username = model.username,
                        fullname = model.fullname,
                        status = true,
                        userrole = "editor",
                        password = CommonData.CommonFunction.CalculateMD5Hash(model.password)
                    };
                    UnitOfWork.userRepository.Add(nuser);
                    UnitOfWork.Commit();
                    return RedirectToAction("UserManager");
                }
                else
                {
                    ViewBag.anno = "Tên người dùng này đã tồn tại";
                    return View();
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(changepassViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.oldpassword == model.password)
                {
                    ViewBag.anno = "Mật khẩu mới không được trùng mật khẩu cũ !";
                    return View();
                }
                else
                {
                    User user = UnitOfWork.userRepository.FindByUsername(User.Identity.Name);
                    if(user != null)
                    {
                        user.password = CommonData.CommonFunction.CalculateMD5Hash(model.password);
                        UnitOfWork.Commit();
                        return RedirectToAction("Logout");
                    }
                }
            }
            return View();
        }
        [Authorize(Roles="admin")]
        public JsonResult changeStatus(int userid,bool state = true)
        {
            string prefix = state ? "Đã bỏ cấm" : "Đã cấm";
            User u = UnitOfWork.userRepository.FindByID(userid);
            if(u.username != "admin")
            {
                u.status = state;
                UnitOfWork.Commit();
                return Json(new { Message = prefix + " \"" + u.username + "\"" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = "Không được cấm admin" }, JsonRequestBehavior.AllowGet);
        }
    }
}