using Project.BAL.Repositories;
using Project.Models.DataMapper;
using ProjectGroup4.Areas.Admin.Models;
using ProjectGroup4.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectGroup4.Areas.Admin.Controllers
{
    public class AttributesController : BaseController
    {
        Repository<Attributes> _Att;
        Repository<AttributeType> _AttType;
        Repository<ProductAttribute> _ProAtt;
        public AttributesController()
        {
            _Att = new Repository<Attributes>();
            _AttType = new Repository<AttributeType>();
            _ProAtt = new Repository<ProductAttribute>();
        }
        // GET: Admin/Attributes
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Index()
        {
            ViewBag.AttType = _AttType.GetAll();
            
            return View(_Att.GetAll().AsQueryable().Include(x => x.AttributeType));
        }

        // GET: Admin/Attributes/Details/5
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            return View(_Att.Get(id));
        }

        // GET: Admin/Attributes/Create
        [CustomAuth(Roles = "ADD")]
        public ActionResult Create()
        {
            ViewBag.AtttypeId = new SelectList(_AttType.GetAll(), "AtttypeId", "TypeName");
            return View();
        }

        // POST: Admin/Attributes/Create
        [CustomAuth(Roles = "ADD")]
        [HttpPost]
        public ActionResult Create(Attributes a)
        {
            ViewBag.AtttypeId = new SelectList(_AttType.GetAll(), "AtttypeId", "TypeName");

            if (ModelState.IsValid)
            {
                if (_Att.Add(a))
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Thêm Thành Công Thuộc Tính Có ID = " + a.AtttypeId,
                    };
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Có lỗi Trong Quá Tạo Mới");
                return View();
            }
            ModelState.AddModelError("", "Có lỗi Trong Quá Tạo Mới");
            return View();
        }

        // GET: Admin/Attributes/Edit/5
        [CustomAuth(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            ViewBag.AtttypeId = new SelectList(_AttType.GetAll(), "AtttypeId", "TypeName");
            if (id == null || id == 0)
            {
                return View("_404");
            }
            else
            {
                return View(_Att.Get(id));
            }
            
        }

        // POST: Admin/Attributes/Edit/5
        [CustomAuth(Roles = "EDIT")]
        [HttpPost]
        public ActionResult Edit(int id, Attributes a)
        {
            if (ModelState.IsValid)
            {
                
                if (_Att.Edit(a))
                    {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Sửa Thành Công Thuộc Tính Có ID = " + a.AttributeId,
                    };
                    return RedirectToAction("Index");
                    }
                ModelState.AddModelError("", "Có lỗi Trong Quá Trình Sửa");
                return View();   
            }
            ModelState.AddModelError("", "Có lỗi Trong Quá Trình Sửa");
            return View();
            
        }

        // GET: Admin/Attributes/Delete/5
        [CustomAuth(Roles = "DELETE")]
        public ActionResult Delete(int? id)
        {
            if (id == null && id == 0)
            {
                return View("_404");

            }
            else {
                try
                {
                    if (_ProAtt.GetBy(x=> x.AttributeId == id).Count() > 0)
                    {
                        TempData["msg"] = new ResponseMessage()
                        {
                            Type = "alert-danger",
                            Message = "Thuộc Tính Này Đang Được Dùng"
                        };
                        return RedirectToAction("Index");
                    }
                    _Att.Remove(id);
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Xóa thành công!"
                    };
                }
                catch (Exception ex)
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-danger",
                        Message = ex.Message
                    };
                }
                return RedirectToAction("Index");
            }
           
        }

        
        
            
        
    }
}
