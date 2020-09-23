using Project.BAL.Repositories;
using Project.Models.DataMapper;
using ProjectGroup4.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectGroup4.Areas.Admin.Controllers
{
    public class BusinessesController : BaseController
    {
        Reflection rfl;
        Repository<Business> _business;
        public BusinessesController()
        {
            rfl = new Reflection();
            _business = new Repository<Business>();
        }
        public ActionResult Index()
        {
            return View(_business.GetAll());
        }
        public ActionResult Update()
        {
            // Lấy tất cả controller trong Admin
            var controllers = rfl.GetControllers("ProjectGroup4.Areas.Admin.Controllers");

            // Insert vào db
            foreach (Type item in controllers)
            {
                var _ctrlName = item.Name.Replace("Controller", "");
                // Kiểm tra nghiệp vụ đã tồn tại hay chưa
                if (!_business.GetAll().Any(x => x.BusinessId == _ctrlName)) // Nếu chưa tồn tại
                {
                    Business b = new Business()
                    {
                        BusinessId = _ctrlName,
                        BusinessName = "Đang cập nhật...."
                    };
                    _business.Add(b);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
