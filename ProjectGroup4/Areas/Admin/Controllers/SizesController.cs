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
    public class SizesController : BaseController
    {
        Repository<Size> _Size;
        Repository<ProductSize> _proSize;
        Repository<OrderDetail> _od;
        public SizesController()
        {
            _Size = new Repository<Size>();
            _proSize = new Repository<ProductSize>();
            _od = new Repository<OrderDetail>();
        }
        // GET: Admin/Sizes
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Index()
        {
            return View(_Size.GetAll());
        }

        // GET: Admin/Sizes/Details/5
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            return View(_Size.Get(id));
        }

        // GET: Admin/Sizes/Create
        [CustomAuth(Roles = "ADD")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sizes/Create
        [CustomAuth(Roles = "ADD")]
        [HttpPost]
        public ActionResult Create(Size s)
        {
            s.CreateAt = DateTime.Now;
            
                if (_Size.GetAll().Any(x => x.SizeName.ToLower().Equals(s.SizeName.ToLower())))
                {
                    ModelState.AddModelError("", "Tên Này Đã Tồn Tại !");
                    return View();
                }
                if (ModelState.IsValid)
                {
                    if (_Size.Add(s))
                    {
                        TempData["msg"] = new ResponseMessage()
                        {
                            Type = "alert-success",
                            Message = "Thêm thành công !"
                        };
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "có lỗi trong quá trình chỉnh sửa");
                    return View();
                }

                return View();
                
            
            
        }

        // GET: Admin/Sizes/Edit/5
        [CustomAuth(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            else
            {
                return View(_Size.Get(id));
            }
            
        }

        // POST: Admin/Sizes/Edit/5
        [CustomAuth(Roles = "EDIT")]
        [HttpPost]
        public ActionResult Edit(int id, Size s )
        {
            s.CreateAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (_Size.Edit(s))
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Thêm thành công !"
                    };
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Có Lỗi Trong Quá Trình Chỉnh Sửa");
                return View();
            }
            
            return View();
        }

        // GET: Admin/Sizes/Delete/5
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
                    if (_proSize.GetBy(x=> x.ProductId == id).Count() > 0)
                    {
                        TempData["msg"] = new ResponseMessage()
                        {
                            Type = "alert-danger",
                            Message = "Kích thước cấp này đang được sử dụng bởi sản phẩm"
                        };
                        return RedirectToAction("Index")
;                    }
                    if (_od.Get(id) != null)
                    {
                        TempData["msg"] = new ResponseMessage()
                        {
                            Type = "alert-danger",
                            Message = "Kích thước cấp này đang được đặt hàng"
                        };
                    }
                    _Size.Remove(id);
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
