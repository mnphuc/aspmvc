using Project.BAL.Repositories;
using Project.Models.DataMapper;
using ProjectGroup4.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProjectGroup4.Controllers
{
    public class ShoppingCartsController : Controller
    {
        Repository<Product> _pro;
        Repository<Color> _color;
        Repository<Size> _s;
        Repository<Attribute> _attributes;
        Repository<Orders> _orders;
        Repository<OrderDetail> _ordersdetail;

        public ShoppingCartsController()
        {
            _pro = new Repository<Product>();
            _color = new Repository<Color>();
            _s = new Repository<Size>();
            _attributes = new Repository<Attribute>();
            _ordersdetail = new Repository<OrderDetail>();

            _orders = new Repository<Orders>();
        }
        // GET: ShoppingCarts
        public ActionResult Index()
        {
            
            List<ItemCart> lst = new List<ItemCart>();
           
            if (Session["cart"] != null)
            {
                // Lấy các sp trong session đưa ta list để thao tác
                lst = Session["cart"] as List<ItemCart>;
            }
            ViewBag.Color = _color.GetAll();
            ViewBag.Size = _s.GetAll();
            return View(lst);
        }
        [HttpPost]
        public ActionResult AddToCart(ItemCart itemCart)
        {
            // Lấy sản phẩm
            int productId = Int32.Parse(Request["ProductId"]);
            int SizeId = Int32.Parse(Request["SizeId"]);
            int ColorId = Int32.Parse(Request["ColorId"]);
            var _product = _pro.Get(productId);
            // Lấy attributes
            //itemCart.Attributes = Request["Attributes"].ToString();

            itemCart.Product = _product;
            itemCart.SizesId = SizeId;
            itemCart.ColorId = ColorId;
            // Thêm vào giỏ hàng
            List<ItemCart> lst = new List<ItemCart>();

            // Kiểm tra giỏ hàng có hay chưa
            if (Session["cart"] != null)// Đã có => Kiểm tra đã trùng sp trong giỏ chưa
            {
                // Lấy các sp trong session đưa ta list để thao tác
                lst = Session["cart"] as List<ItemCart>;
                var check = false;
                foreach (var item in lst)
                {
                    // Trùng =>  Cập nhật số lượng
                    if (item.Product.ProductId == productId && item.ColorId == ColorId && item.SizesId == SizeId && item.Attributes == itemCart.Attributes)
                    {
                        item.Quantity += itemCart.Quantity;
                        check = true;
                    }
                }
                // Chưa trùng thì thêm mới
                if (!check)
                {
                    lst.Add(itemCart);
                }
            }
            else // Chưa có => thêm mới sp và lưu vào giỏ
            {
                lst.Add(itemCart);
            }
            // Lưu lại giỏ hàng vào session
            Session["cart"] = lst;

            return RedirectToAction("Index");
        }
        public ActionResult Payment()
        {
            string msg;
            // Kiểm tra đăng nhập

            
            // Kiểm tra có giỏ hàng hay không
            if (Session["cart"] == null)
            {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "error",
                    Message = "Giỏ Hàng Trống",
                };
                return RedirectToAction("Index");
            }
            if (Session["cus"] == null)
            {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "error",
                    Message = "Bạn Chưa Đăng Nhập Vui Lòng Đăng Nhập",
                };
                return RedirectToAction("Login", "Accounts");
            }
            // Kiểm tra giỏ hàng có sp hay không
            List<ItemCart> lst = new List<ItemCart>();
            // Lấy các sp trong session đưa ta list để thao tác
            lst = Session["cart"] as List<ItemCart>;
            if (lst.Count == 0)
            {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "error",
                    Message = "Giỏ Hàng Trống",
                };
                return RedirectToAction("Index");
            }
            ViewBag.Color = _color.GetAll();
            ViewBag.Size = _s.GetAll();
            return View();
        }
        [HttpPost]
        public ActionResult Payment(Orders order)
        {
            var user = (Customer)Session["cus"];
            
            #region TẠO THÔNG TIN CHO ĐỐI TƯỢNG ORDER
            order.CreateDate = DateTime.Now;
            // Mặc định mới thêm là 1: Đang chờ duyệt
            order.Status = 1;
            // Đưa OrderDetails vào Order
            // Kiểm tra giỏ hàng có sp hay không
            List<ItemCart> lst = new List<ItemCart>();
            // Lấy các sp trong session đưa ta list để thao tác
            lst = Session["cart"] as List<ItemCart>;
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in lst)
            {
                OrderDetail od = new OrderDetail
                {
                    ProductId = item.Product.ProductId,
                    Quantity = item.Quantity,
                    SizeId = item.SizesId,
                    ColorId = item.ColorId,
                    Price = (item.Product.PriceOut * (100 - item.Product.Discount) / 100)
                };
                orderDetails.Add(od);
            }
            order.OrderDetails = orderDetails;
            #endregion TẠO THÔNG TIN CHO ĐỐI TƯỢNG ORDER




            #region THÊM GIỎ HÀNG
            // Inser giỏ hàng
            if (!_orders.Add(order))
            {
                return View();
            }
            #endregion

            // Gửi Email
            // Tạo nội dung email
            string _header = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/header_order.txt"));
            string _footer = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/footer_order.txt"));
            #region Thông tin người nhận
            string _main = String.Format(@"<h2 class='title'>ĐƠN HÀNG BKAP{5}</h2>
				<p>
					<b>Họ tên người nhận</b>
					<span>{0}</span>
				</p>
				<p>
					<b>Email</b>
					<span>{1}</span>
				</p>
				<p>
					<b>SĐT</b>
					<span>{2}</span>
				</p>
				<p>
					<b>Địa chỉ</b>
					<span>{3}</span>
				</p>
				<p>
					<b>Ngày mua</b>
					<span>{4:dd/MM/yyyy | HH:mm:ss}</span>
				</p>", order.FullName, order.Email, order.Phone, order.Address, order.CreateDate, order.OrderId);
            #endregion
            #region Sản phẩm mua
             
                double total = 0;
                double gg = 0;
                double tructra = 0;
            
            _main += @"<table class='table text-center'>
					<thead>
					    <th>Sản phẩm</th>
                        <th>Số lượng</th>
                        <th>Giảm giá</th>
                        <th>Đơn giá</th>
                        <th>Thực trả</th>
                        <th>Thành Tiền</th>
                        
                    </thead>
					<tbody>";
            foreach (var item in lst)
            {
                gg = (item.Product.Discount) * (item.Product.PriceOut) / 100;
                tructra = item.Product.PriceOut - gg;
                
                _main += @"<tr>";
                _main += String.Format(@"<td>{0}</td>", item.Product.ProductName);
                _main += String.Format(@"<td>{0}</td>", item.Quantity);
                _main += String.Format(@"<td>{0}</td>", item.Product.Discount+"%");
                _main += String.Format(@"<td>{0:#,##} VNĐ</td>", item.Product.PriceOut);
                _main += String.Format(@"<td>{0:#,##} VNĐ</td>", tructra);
                _main += String.Format(@"<td>{0:#,##} VNĐ</td>", tructra * item.Quantity);
                _main += @"</tr>";
                total += (item.Quantity * tructra);
            }
            _main += String.Format(@"
                            <tr>
							<th colspan='4' style='text-align: left'>Tổng tiền </th>
							<th>{0}</th>
						</tr>
					</tbody>
				</table>", String.Format(@"<td>{0:#,##} VNĐ</td>", total));
            #endregion
            Helpers.SendEmail(order.Email, "quantrivienwebsite.bkap@gmail.com", "quantriwebbkap", "[BKAPSHOP]_THÔNG TIN ĐƠN HÀNG", _header + _main + _footer);
            TempData["msg"] = new ResponseMessage()
            {
                Type = "succes",
                Message = "Đặt Hàng Thành Công",
            };
            Session.Remove("cart");
            return RedirectToAction("SuccessPayment", "Home");
        }

        public ActionResult DeleteCart(int productId, int SizeId, int ColorId,string Attributes)
        {
           

            
            List<ItemCart> lst = new List<ItemCart>();
            lst = Session["cart"] as List<ItemCart>;
            if (lst.Count() == 0)
            {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "error",
                    Message = "Không Tồn Tại Đơn Hàng",
                };
                return View("_404");
            }
            foreach (var item in lst.ToList())
            {
                // Trùng =>  Cập nhật số lượng
                if (item.Product.ProductId == productId && item.ColorId == ColorId && item.SizesId == SizeId && item.Attributes == Attributes)
                {
                    lst.Remove(item);
                }
            }
            TempData["msg"] = new ResponseMessage()
            {
                Type = "succes",
                Message = "Xóa Đơn Hàng Thành Công",
            };
            return RedirectToAction("Index");
        }
        public ActionResult UpdateCart(int productId, int qty, int ColorId, int SizeId)
        {

            List<ItemCart> lst = new List<ItemCart>();
            lst = Session["cart"] as List<ItemCart>;
            foreach (var item in lst.ToList())
            {
                // Trùng =>  Cập nhật số lượng
                if (item.Product.ProductId == productId && item.ColorId == ColorId && item.SizesId == SizeId)
                {
                    //item.Quantity += itemCart.Quantity;
                    item.Quantity = qty;
                }

            }
            Session["cart"] = lst;
            double totalPro = 0;
            double price = 0;
            double totalDetail = 0;
            foreach (var item in lst.ToList())
            {
                price = item.Product.PriceOut * (100 - item.Product.Discount) / 100;
                totalPro = price * item.Quantity;
                totalDetail += totalPro;

            }
            return Json(new { totalProduct = price, TotalDetail = totalDetail });
        }
    }
}