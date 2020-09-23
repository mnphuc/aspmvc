using Project.BAL.Repositories;
using Project.Models.DataMapper;
using Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectGroup4.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        Repository<Users> _user;
        public LoginController()
        {
            _user = new Repository<Users>();
        }
        public ActionResult Login()
        {
            if (Session["user"] != null)
            {
                return RedirectToRoute(new {Controller = "Home", Action = "Index" });
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLogin u)
        {
            if (ModelState.IsValid)
            {
                var _u = _user.SingleBy(x => x.Email == u.UserName && x.Password == u.Password);
                if(_u == null)
                {
                    ModelState.AddModelError("", "Sai Tài Khoản Mật Khẩu");
                    return View();
                }
                if (_u != null) // Đăng nhập thành công!
                {
                    Session["user"] = _u;
                    return RedirectToRoute(new { Controller = "Home", Action = "Index"});
                }
                return View();
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("user");
            return RedirectToAction("Index", "Home");
        }
    }
}