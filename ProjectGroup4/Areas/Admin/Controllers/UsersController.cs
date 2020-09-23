using Project.BAL.Repositories;
using Project.Models.DataMapper;
using ProjectGroup4.Areas.Admin.Models;
using ProjectGroup4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectGroup4.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        Repository<Users> _User;
        Repository<Group> _Group;
        public UsersController()
        {
            _User = new Repository<Users>();
            _Group = new Repository<Group>();
        }
        // GET: Admin/Users
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Index()
        {
            return View(_User.GetAll());
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details()
        {
            var user = (Users)Session["user"];
            return View(_User.Get(user.UserId));
        }

        // GET: Admin/Users/Create
        [CustomAuth(Roles = "ADD")]
        public ActionResult Create()
        {

            ViewBag.GroupId = new SelectList(_Group.GetAll(), "GroupId", "GroupName");
            return View();
        }

        // POST: Admin/Users/Create
        [CustomAuth(Roles = "ADD")]
        [HttpPost]
        public ActionResult Create(Users u)
        {
            u.CreateDate = DateTime.Now;
            ViewBag.GroupId = new SelectList(_Group.GetAll(), "GroupId", "GroupName");
            if (ModelState.IsValid)
            {
                if (_User.GetAll().Any(x => x.Email.ToLower().Equals(u.Email.ToLower())))
                {
                    ModelState.AddModelError("", "Tên Này Đã Tồn Tại !");
                    return View();
                }
                if (_User.Add(u))
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Thêm Thành Người Dùng Có ID = " + u.FullName,
                    };
                    return RedirectToAction("index");
                }
                ModelState.AddModelError("", "Có Lỗi Trong Quá Trình Thêm Mới");
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult ResetPass()
        {
            var user = (Users)Session["user"];
            if(user == null)
            {
                return View("_404");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ResetPass(FormCollection collection)
        {
                var user = (Users)Session["user"];
                if(user.Password != collection.Get("password"))
                {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "alert-error",
                    Message = "Sai Mật Khẩu Cũ",
                };
                return View();
                }
                if (collection.Get("passWordNew").Length < 8)
            {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "alert-error",
                    Message = "Mật Khẩu Mới Phải Lớn Hơn 8 ký Tự",
                };
                return View();
            }
                if (collection.Get("passWordNew") != collection.Get("passwordCheck"))
                {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "alert-error",
                    Message = "Mật Khẩu Mới Không Giống nhau",
                };
                return View();
                }
                

                user.Password = collection.Get("passWordNew");
            if (_User.Edit(user))
            {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "alert-success",
                    Message = "Đổi Mật Khẩu Thành Công",
                };
                return Redirect("Index");

            }
            return View();
                
           
               
            
           
                 
        
        }
        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.GroupId = new SelectList(_Group.GetAll(), "GroupId", "GroupName");
            return View();
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
