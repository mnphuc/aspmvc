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
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        Repository<Users> _user;
        Repository<Customer> _cus;
        Repository<Orders> _od;
        Repository<Product> _pro;
        Repository<OrderDetail> _odd;
        public HomeController()
        {
            _user = new Repository<Users>();
            _cus = new Repository<Customer>();
            _od = new Repository<Orders>();
            _pro = new Repository<Product>();
            _odd = new Repository<OrderDetail>();
        }
        public ActionResult Index()
        {
            var ordd = _odd.GetAll();
            double total = 0;
            ViewBag.countCus = _cus.GetAll().Count();
            ViewBag.countOrder = _od.GetAll().Count();
            ViewBag.countProduct = _pro.GetAll().Count();
      
            foreach(var i in ordd)
            {
                total += (i.Quantity * i.Price);
            }
            
            ViewBag.total = total;
            return View();
        }
        public ActionResult Customers()
        {


            return View(_cus.GetAll());
        }
        
    }
}