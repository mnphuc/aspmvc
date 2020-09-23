using Project.BAL.Repositories;
using Project.Models.DataMapper;
using Project.Models.ViewModels;
using ProjectGroup4.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project.Models.ViewModels.CustomerViewModel;

namespace ProjectGroup4.Controllers
{
    public class AccountsController : Controller
    {
        Repository<Customer> _customers;
        Repository<Orders> _orders;
        Repository<OrderDetail> _orderDetails;
        Repository<Product> _products;

        
        public AccountsController()
        {
            _customers = new Repository<Customer>();
            _orders = new Repository<Orders>();
            _orderDetails = new Repository<OrderDetail>();
            _products = new Repository<Product>();
        }
        // GET: Accounts
        public ActionResult Index()
        {
            var data = (Customer)Session["cus"];
            if(data == null)
            {
                return View("_404");
            }
            return View(_customers.Get(data.CustomerId));
        }

        // Login
        public ActionResult Login()
        {
            if (Session["cus"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(CustomerViewModel.CustomerLogin c)
        {
            if (ModelState.IsValid)
            {
                var cus = _customers.SingleBy(x => x.Email == c.Email.ToLower() && x.Password == c.Password);
                
                if (cus != null) // Tìm thấy
                {
                    if (cus.Active != "active")
                    {
                        Session["check"] = cus;
                        return RedirectToAction("activeCode");
                    }
                    Session["cus"] = cus;
                    return RedirectToAction("Index", "Home");
                }
                    ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                    return View();
                
            }
            return View();
        }
        // register

        public ActionResult Register()
        {
            if (Session["cus"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer c)
        {
            c.CreateDate = DateTime.Now;
            c.Status = true;
            Random rand = new Random(6);
            int txt = rand.Next(0, 999999);
            c.Active = Convert.ToString(txt);
            if (ModelState.IsValid)
            {
                c.Email = c.Email.ToLower();
                var file = Request.Files["Avartar"];
                if (file!=null&&file.ContentLength>0)
                {
                    string fileName = file.FileName;
                    // tạo đường dẫn lưu file
                    string path = Server.MapPath("~/Content/Upload/imageCus/") + fileName;
                    // lưu
                    file.SaveAs(path);
                    // gán avatar  bằng đường dẫn
                    c.Avartar = fileName;
                }
                var checkList = _customers.GetAll().AsQueryable().Where(x => x.Email.ToLower() == c.Email.ToLower());
                if (checkList != null)
                {
                    ModelState.AddModelError("", "Email Này Đã Được Sử Dụng");
                    return View();
                }
                if (_customers.Add(c))
                {
                    Session["check"] = c;
                    sendCode();
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "succes",
                        Message = "Mã Kích Hoạt Đã Được Gửi Đến Mail Của Bạn",
                    };
                    return RedirectToAction("activeCode");
                };
                return View();
            }
            return View();
        }
        public ActionResult resertPass()
        {
            var cus = (Customer)Session["cus"];
            if(cus != null)
            {
                return View("_404");
            }
            return View();
        }
        [HttpPost]
        public ActionResult resertPass(FormCollection collection)
        {
            string email = collection.Get("EmailResert");
            var check = _customers.GetAll().AsQueryable().Where(x => x.Email == email);
            if (email == null)
            {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "alert-error",
                    Message = "bạn Chưa Nhập Email",
                };
                return View();
            }
            if(check == null)
            {
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "error",
                    Message = "Tài Khoản Email Không Tồn Tại",
                };
                return View();
            }

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            string pas = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray()).ToLower();
            Customer cus = _customers.SingleBy(x => x.Email.ToLower() == email);
            cus.Password = pas;
            cus.Active = "active";
            if (_customers.Edit(cus))
            {
                string _header = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/header_order.txt"));
                string _footer = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/footer_order.txt"));
                string _main = String.Format(@"<h2 class='title'>Mật Khẩu Mới Của Bạn Là</h2>
				<p>
					<h2 class='title'>{0}</h2>
				</p>
				", cus.Password);
                Helpers.SendEmail(cus.Email, "quantrivienwebsite.bkap@gmail.com", "quantriwebbkap", "[BKAPSHOP]_Khôi Phục Tài Khoản", _header + _main + _footer);
                TempData["msg"] = new ResponseMessage()
                {
                    Type = "succes",
                    Message = "Mật Khẩu Đã Được Gửi Đến Mail Của Bạn",
                };
                return RedirectToAction("index", "Home");
            }
            
            return View();
        }
        [HttpGet]
        public ActionResult sendCode()
        {
            var check = (Customer)Session["check"];
            string _header = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/header_order.txt"));
            string _footer = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/footer_order.txt"));
            string _main = String.Format(@"<h2 class='title'>Mã Kích Hoạt Tài Khoản</h2>
				<p>
					<h2 class='title'>{0}</h2>
				</p>
				", check.Active);
            Helpers.SendEmail(check.Email, "quantrivienwebsite.bkap@gmail.com", "quantriwebbkap", "[BKAPSHOP]_Kích Hoạt Tài Khoản", _header + _main + _footer);

            return Json("Thành Công", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult activeCode()
        {
            var check = (Customer)Session["check"];
           
            if (check == null)
            {
                return View("_404");
            }
            return View();
        }
        [HttpPost]
        public ActionResult activeCode(FormCollection collection)
        {
            var check = (Customer)Session["check"];
            var code = collection.Get("activeCode");
                if (check.Active != code)
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                    Type = "error",
                    Message = "Mã Kích Hoạt Không Chính Xác",
                    };
                return View();
                }
                check.Active = "active";
                 check.Update_at = DateTime.Now;
                    Customer cus = _customers.Get(check.CustomerId);
                    cus.Active = "active";
                    
                  if (_customers.Edit(cus))
                    {
                
                        Session["cus"] = check;
                        Session.Remove("check");
                        return RedirectToAction("Index", "Home");
                    }

                  TempData["msg"] = new ResponseMessage()
                    {
                         Type = "error",
                       Message = "Có Lỗi Trong Quá Trình Xử Lý",
                    };
                return View();
            
        }
        // edit
        public ActionResult Edit()
        {
            var user = (Customer)Session["cus"];
            if (user==null)
            {
                return View("_404");
            }
       
            return View(_customers.Get(user.CustomerId));
        }

        [HttpPost]
        public ActionResult Edit(Customer c )
        {
            c.CreateDate = DateTime.Now;
            c.Status = true;
            c.Active = "active";
            c.Update_at = DateTime.Now;
            
           
            if (ModelState.IsValid)
            {
                c.Email = c.Email.ToLower();
                var file = Request.Files["Avartar"];
                if (file != null && file.ContentLength > 0)
                {
                   
                    string fileName = file.FileName;
                    // tạo đường dẫn lưu file
                   
                    // vì sửa nên kiểm tra ảnh đã tồn tại chưa, nếu chưa thì =>lưu, nếu tồn tại rồi thì k lưu nữa
                    if (!Directory.Exists(Server.MapPath(@"~/Content/Upload/imageCus/") + fileName))
                    {
                        // lưu
                        string path = Server.MapPath("~/Content/Upload/imageCus/") + fileName;
                        file.SaveAs(path);
                    }
                   
                    // gán avatar  bằng đường dẫn
                    c.Avartar = fileName;
                    
                }
                if (_customers.Edit(c))
                {
                    Session["cus"] = c;
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "succes",
                        Message = "Thay Đổi Thông Tin Thành Công",
                    };
                    return RedirectToAction("Index");
                };
                return View();
            }
            return View();
        }
        //logout
        public ActionResult Logout()
        {
            Session.Remove("cus");
            TempData["msg"] = new ResponseMessage()
            {
                Type = "succes",
                Message = "Đăng Xuất Thành Công",
            };
            return RedirectToAction("Index", "Home");
        }



        // order information
        public ActionResult Details()
        {
            var user = (Customer)Session["cus"];
            if(user == null)
            {
                return View("_404");
            }
            var data = _orders.GetAll().AsQueryable().Where(x => x.CustomerId == user.CustomerId);
            return View(data);
        }
    }

   
}