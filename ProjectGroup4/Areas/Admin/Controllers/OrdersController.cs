using Project.BAL.Repositories;
using Project.Models.DataMapper;
using Project.Models.ViewModels;
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
    public class OrdersController : BaseController
    {
        Repository<Orders> _orders;
        Repository<OrderDetail> _orderDetails;
        Repository<Product> _products;
        Repository<Color> _co;
        Repository<Size> _size;
        public OrdersController()
        {
            _orders = new Repository<Orders>();
            _orderDetails = new Repository<OrderDetail>();
            _products = new Repository<Product>();
            _co = new Repository<Color>();
            _size = new Repository<Size>();
        }
        // GET: Admin/Orders
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Index()
        {
            var data = _orders.GetAll().AsQueryable().OrderByDescending(x => x.CreateDate).Include(x => x.Customer).ToList();
            return View(data);
        }



        [CustomAuth(Roles = "VIEW")]
        public ActionResult Details(int id)
        {
            // Thông tin Order (thông tin mua hàng) và thông tin sản phẩm đã mua
            //.Include(x => x.OrderDetails) // Chi tiết đơn hàng

            var order = _orders.GetAll().AsQueryable().Include(x => x.Customer).FirstOrDefault(x => x.OrderId == id);
            var orderDetails = _orderDetails.GetBy(x => x.OrderId == id);
            List<OrderDetailsViewModel> listODVM = new List<OrderDetailsViewModel>();
            foreach (var item in orderDetails)
            {
                OrderDetailsViewModel odv = new OrderDetailsViewModel();
                odv.Product = _products.Get(item.ProductId);
                odv.Quantity = item.Quantity;
                odv.Price = item.Price;
                odv.Money = item.Quantity * item.Price;
                listODVM.Add(odv);


               
            }
            
            ViewBag.orderDetails = listODVM;
            return View(order);
        }



        [CustomAuth(Roles = "VIEW")]
        public ActionResult Update(int id, int status)
        {
            var order = _orders.Get(id);
            // Trường hợp ko hợp lệ
            if (order.Status > status && order.Status != 1)
            {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "callout-danger",
                    Message = "Cập nhật không hợp lệ!"
                };
                return RedirectToAction("Details", new { id = id });
            }
            order.Status = Convert.ToByte(status);
            _orders.Save();
            return RedirectToAction("Index");
        }



    }
}
