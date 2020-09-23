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
    public class SliderController : BaseController
    {
        Repository<Slider> _Slider;
        public SliderController()
        {
            _Slider = new Repository<Slider>();
        }
        // GET: Admin/Slider
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Index()
        {
            return View(_Slider.GetAll());
        }

        // GET: Admin/Slider/Details/5
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            return View(_Slider.Get(id));
        }

        // GET: Admin/Slider/Create
        [CustomAuth(Roles = "ADD")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Slider/Create
        [CustomAuth(Roles = "ADD")]
        [HttpPost]
        public ActionResult Create(Slider s)
        {
            s.CreateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (_Slider.GetAll().Any(x => x.SliderName.ToLower().Equals(s.SliderName.ToLower())))
                {
                    ModelState.AddModelError("", "Tên Này Đã Tồn Tại !");
                    return View();
                }
                if (_Slider.Add(s) == true)
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Thêm Thành Công Kiểu Thuộc Tính Có ID = " + s.SliderId,
                    };
                    return RedirectToAction("Index");

                }
                ModelState.AddModelError("", "Có lỗi trong quá trình thêm mới");
                return View();
            }
            return View();
        }

        // GET: Admin/Slider/Edit/5
        [CustomAuth(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return View("_404");
            }
            else
            {
                return View(_Slider.Get(id));
            }

        }

        // POST: Admin/Slider/Edit/5
        [CustomAuth(Roles = "EDIT")]
        [HttpPost]
        public ActionResult Edit(Slider s)
        {
            s.CreateDate = DateTime.Now;
            if (ModelState.IsValid)
            {

                if (_Slider.Edit(s))
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Sửa thành công !"
                    };
                    return RedirectToAction("index");
                }
                ModelState.AddModelError("", "Có lỗi trong quá trình chỉnh sửa");
                return View();
            }

            return View();
        }

        // GET: Admin/Slider/Delete/5
        [CustomAuth(Roles = "DELETE")]
        public ActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            else
            {
                try
                {

                    _Slider.Remove(id);
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Xóa thành công!"
                    };
                }
                catch (Exception)
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-danger",
                        Message = "Có Lỗi Trong Quá Trình Xóa"
                    };
                }
                return RedirectToAction("Index");
            }
        }


    }
}
