using Project.BAL.Repositories;
using Project.Models.DataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectGroup4.Controllers
{
    public class HomeController : Controller
    {
        Repository<Category> _cat;
        Repository<Slider> _slider;
        public HomeController()
        {
            //khởi tạo (lấy dữ liệu)
            _cat = new Repository<Category>();

            _slider = new Repository<Slider>();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Banner()
        {
            var data = _slider.GetBy(x => x.Status == true).Take(3);
            return View(data);
        }

        public ActionResult Blog(int? id)
        {


            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }
        public PartialViewResult MainMenu()
        {
            var data = _cat.GetBy(x => x.ShowHome == true);

            return PartialView("_MainMenu", data);
        }
        public PartialViewResult getAbout()
        {
            var data = _cat.GetBy(x => x.ShowHome == true).OrderBy(x => x.DisplayNo).Take(3);
            return PartialView("_getAbout", data);
        }
        public ActionResult SuccessPayment()
        {
            if(Session["cus"] == null)
            {
                return View("_404");
            }
            return View();
        }
    }
}