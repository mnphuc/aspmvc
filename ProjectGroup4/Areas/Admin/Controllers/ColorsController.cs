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
    public class ColorsController : BaseController
    {
        Repository<Color> _color;
        Repository<ProductColor> _proC;
        Repository<OrderDetail> _orD;
        public ColorsController()
        {
            _color = new Repository<Color>();
            _proC = new Repository<ProductColor>();
            _orD = new Repository<OrderDetail>();
        }
        // GET: Admin/Colors
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Index()
        {
            return View(_color.GetAll());
        }

        // GET: Admin/Colors/Details/5
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            return View(_color.Get(id));
        }

        // GET: Admin/Colors/Create
        [CustomAuth(Roles = "ADD")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Colors/Create
        [CustomAuth(Roles = "ADD")]
        [HttpPost]
        public ActionResult Create(Color c)
        {
            c.CreateAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (_color.GetAll().Any(x => x.ColorName.ToLower().Equals(c.ColorName.ToLower())))
                {
                    ModelState.AddModelError("", "Tên Này Đã Tồn Tại !");
                    return View();
                }
                if (_color.Add(c))
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Thêm Thành Công ID = " + c.ColorId,
                    };
                    return RedirectToAction("index");
                }
                ModelState.AddModelError("", "Có Lỗi Trong Quá Trình Thêm Mới");
                return View();
            }
            return View();
        }

        // GET: Admin/Colors/Edit/5
        [CustomAuth(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            else
            {
                return View(_color.Get(id));
            }
            
        }

        // POST: Admin/Colors/Edit/5
        [CustomAuth(Roles = "EDIT")]
        [HttpPost]
        public ActionResult Edit(int id, Color c)
        {
            c.CreateAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                
                if (_color.Edit(c))
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Sửa Thành Công Thuộc Tính Có ID = " + c.ColorId,
                    };
                    return RedirectToAction("index");
                }
                ModelState.AddModelError("", "Có Lỗi Trong Quá Trình Sửa");
                return View();
            }
            return View();
        }

        // GET: Admin/Colors/Delete/5
        [CustomAuth(Roles = "DELETE")]
        public ActionResult Delete(int? id)
        {
            if (id == null || id ==0)
            {
                return View("_404");
            }
            else
            {
                try
                {
                    if (_proC.GetBy(x=>x.ColorId == id).Count() > 0)
                    {
                        TempData["msg"] = new ResponseMessage()
                        {
                            Type = "alert-danger",
                            Message = "Màu này đang được dùng với sản phẩm"
                        };
                        return RedirectToAction("Index");
                    }
                    if (_orD.GetBy(x=> x.ColorId == id).Count() >  0)
                    {
                        TempData["msg"] = new ResponseMessage()
                        {
                            Type = "alert-danger",
                            Message = "Màu này đã có trong đơn hàng của khách"
                        };
                        return RedirectToAction("Index");
                    }
                    _color.Remove(id);
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
