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
    public class ProvidersController : BaseController
    {
        Repository<Provider> _Provider;
        Repository<Product> _pro;
        public ProvidersController()
        {
            _Provider = new Repository<Provider>();
            _pro = new Repository<Product>();
        }
        // GET: Admin/Providers
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Index()
        {
            return View(_Provider.GetAll());
        }

        // GET: Admin/Providers/Details/5
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            return View(_Provider.Get(id));
        }

        // GET: Admin/Providers/Create
        [CustomAuth(Roles = "ADD")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Providers/Create
        [CustomAuth(Roles = "ADD")]
        [HttpPost]
        public ActionResult Create(Provider p)
        {
            if (ModelState.IsValid)
            {
                if (_Provider.GetAll().Any(x => x.ProviderName.ToLower().Equals(p.ProviderName.ToLower())))
                {
                    ModelState.AddModelError("", "Tên Này Đã Tồn Tại !");
                    return View();
                }
                if (_Provider.Add(p))
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Thêm thành công !"
                    };
                    return RedirectToAction("index");
                }
                ModelState.AddModelError("", "Có lỗi trong quá trình thêm mới !");
                return View();
            }
            return View();
        }

        // GET: Admin/Providers/Edit/5
        [CustomAuth(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            else
            {
                return View(_Provider.Get(id));
            }
            
        }

        // POST: Admin/Providers/Edit/5
        [CustomAuth(Roles = "EDIT")]
        [HttpPost]
        public ActionResult Edit(int id, Provider p)
        {
            if (ModelState.IsValid)
            {
                
                if (_Provider.Edit(p))
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Sửa thành công !"
                    };
                    return RedirectToAction("index");
                }
                ModelState.AddModelError("", "Có lỗi trong quá  trình thêm mới");
                return View();
            }
            
            return View();
        }

        // GET: Admin/Providers/Delete/5
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
                    if (_pro.Get(id) !=null)
                    {
                        TempData["msg"] = new ResponseMessage()
                        {
                            Type = "alert-danger",
                            Message = "Nhà cung cấp này đang được sử dụng bởi sản phẩm"
                        };
                        return RedirectToAction("Index");
                    }   
                    _Provider.Remove(id);
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
