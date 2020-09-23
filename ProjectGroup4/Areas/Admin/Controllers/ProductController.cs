using Project.BAL.Repositories; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models.DataMapper;
using System.Data.Entity;
using System.Text.RegularExpressions;
using ProjectGroup4.Models;
using ProjectGroup4.Areas.Admin.Models;

namespace ProjectGroup4.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        Repository<Product> _Product;
        Repository<Provider> _Provider;
        Repository<CategoryProduct> _CategoryProduct;
        Repository<Category> _Category;
        Repository<Color> _Color;
        Repository<Size> _Size;
        Repository<Attributes> _Attribute;
        Repository<AttributeType> _Attr;
        Repository<ProductAttribute> _proAtt;
        Repository<OrderDetail> _od;
        Repository<ProductSize> _proSize;
        Repository<ProductColor> _proColor;
        // GET: Admin/Product
        public ProductController()
        {
            _Product = new Repository<Product>();
            _Provider = new Repository<Provider>();
            _CategoryProduct = new Repository<CategoryProduct>();
            _Category = new Repository<Category>();
            _Color = new Repository<Color>();
            _Size = new Repository<Size>();
            _Attribute = new Repository<Attributes>();
            _Attr = new Repository<AttributeType>();
            _proAtt = new Repository<ProductAttribute>();
            _od = new Repository<OrderDetail>();
            _proSize = new Repository<ProductSize>();
            _proColor = new Repository<ProductColor>();
            
        }

        [CustomAuth(Roles = "VIEW")]
        public ActionResult Index()
        {
            ViewBag.Category = _Category.GetAll();
            return View(_Product.GetAll());
        }
        [CustomAuth(Roles = "VIEW")]
        public ActionResult Details(int? id)
        {
            if (id == null || id== 0)
            {
                return View("_404");
            }
            return View(_Product.Get(id));
        }

        // thêm
        [CustomAuth(Roles = "ADD")]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ProviderId = new SelectList(_Provider.GetAll(), "ProviderId", "ProviderName");
            ViewBag.CatPro = _Category.GetAll();
            ViewBag.Color = _Color.GetAll();
            ViewBag.Size = _Size.GetAll();
            ViewBag.Attributse = _Attribute.GetAll();
            ViewBag.AttrTypes = _Attr.GetAll().AsQueryable().Include(x => x.Attributes).AsEnumerable();
            return View();
        }
        [CustomAuth(Roles = "ADD")]
        [HttpPost]
        public ActionResult Create(Product p)
        {
            p.CreateDate = DateTime.Now;
            string url = Slug.GenerateSlug(p.ProductName);
            p.MetaLink = url;
            p.CoutView = 0;
            ViewBag.ProviderId = new SelectList(_Provider.GetAll(), "ProviderId", "ProviderName");
            ViewBag.CatPro = _Category.GetAll();
            ViewBag.Color = _Color.GetAll();
            ViewBag.Size = _Size.GetAll();
            ViewBag.Attribute = _Attribute.GetAll();
            ViewBag.AttrTypes = _Attr.GetAll().AsQueryable().Include(x => x.Attributes).AsEnumerable();
            if (p.CategoryProducts != null)
            {
                p.CategoryProducts.ToList().ForEach(x => x.ProductId = p.ProductId);
            }
            if(p.ProductSizes != null)
            {
                p.ProductSizes.ToList().ForEach(x => x.ProductId = p.ProductId);
            }
            if (p.ProductColors != null)
            {
                p.ProductColors.ToList().ForEach(x => x.ProductId = p.ProductId);
            }
            
            if (p.ProductAttributes != null)
            {
                p.ProductAttributes.ToList().ForEach(x => x.ProductId = p.ProductId);
            }
            if (ModelState.IsValid)
            {
                if (_Product.GetAll().Any(x => x.ProductName.ToLower().Equals(p.ProductName.ToLower())))
                {
                    ModelState.AddModelError("", "Tên Này Đã Tồn Tại !");
                    return View();
                }
                if (_Product.Add(p))
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-success",
                        Message = "Thêm Thành Công Sản Phẩm Có ID = " + p.ProductId,
                    };
                    return RedirectToAction("index");
                }
                ModelState.AddModelError("", "Có Lỗi Trong Quá Trình Thêm Mới");
                return View();
            }
            return View();
        }
        [CustomAuth(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            else
            {
                ViewBag.ProviderId = new SelectList(_Provider.GetAll(), "ProviderId", "ProviderName");
                ViewBag.CatPro = _Category.GetAll();
                ViewBag.Color = _Color.GetAll();
                ViewBag.Size = _Size.GetAll();
                ViewBag.Attribute = _Attribute.GetAll();
                ViewBag.CatProduct = _CategoryProduct.GetBy(x => x.ProductId == id);
                ViewBag.AttrTypes = _Attr.GetAll().AsQueryable().Include(x => x.Attributes).AsEnumerable();
                ViewBag.getCatPro = _Product.GetAll().AsQueryable().Include(x => x.CategoryProducts).FirstOrDefault(x => x.ProductId == id);
                ViewBag.getProColor = _Product.GetAll().AsQueryable().Include(x => x.ProductColors).FirstOrDefault(x => x.ProductId == id);
                ViewBag.getProSize = _Product.GetAll().AsQueryable().Include(x => x.ProductSizes).FirstOrDefault(x => x.ProductId == id);
                return View(_Product.Get(id));
            }
            
        }
        [CustomAuth(Roles = "EDIT")]
        [HttpPost]
        public ActionResult Edit(int id, Product p)
        {
            
            

            ViewBag.ProviderId = new SelectList(_Provider.GetAll(), "ProviderId", "ProviderName");
            ViewBag.CatPro = _Category.GetAll();
            ViewBag.Color = _Color.GetAll();
            ViewBag.Size = _Size.GetAll();
            ViewBag.Attribute = _Attribute.GetAll();
            ViewBag.CatProduct = _CategoryProduct.GetBy(x => x.ProductId == id);
            ViewBag.AttrTypes = _Attr.GetAll().AsQueryable().Include(x => x.Attributes).AsEnumerable();
            

            if (p.CategoryProducts != null)
            {
                _CategoryProduct.RemoveRange(_CategoryProduct.GetBy(x => x.ProductId == p.ProductId));
                p.CategoryProducts.ToList().ForEach(x => x.ProductId = p.ProductId);
                _CategoryProduct.AddRange(p.CategoryProducts);
            }
            if (p.ProductColors != null)
            {
                _proColor.RemoveRange(_proColor.GetBy(x => x.ProductId == p.ProductId));
                p.ProductColors.ToList().ForEach(x => x.ProductId = p.ProductId);
                _proColor.AddRange(p.ProductColors);
            }
            if (p.ProductSizes != null)
            {
                _proSize.RemoveRange(_proSize.GetBy(x => x.ProductId == p.ProductId));
                p.ProductSizes.ToList().ForEach(x => x.ProductId = p.ProductId);
                _proSize.AddRange(p.ProductSizes);
            }
            if (p.ProductAttributes != null)
            {
                _proAtt.RemoveRange(_proAtt.GetBy(x => x.ProductId == p.ProductId));
                p.ProductAttributes.ToList().ForEach(x => x.ProductId = p.ProductId);
                _proAtt.AddRange(p.ProductAttributes);
            }
            if (ModelState.IsValid)
            {

                p.CreateDate = DateTime.Now;
                string url = Slug.GenerateSlug(p.ProductName);
                p.MetaLink = url;
                p.CoutView = 1;
                try
                {
                    if (_Product.Edit(p))
                    {
                        return RedirectToAction("Index");
                    }
                    ViewBag.getCatPro = _Product.GetAll().AsQueryable().Include(x => x.CategoryProducts).FirstOrDefault(x => x.ProductId == id);
                    ViewBag.getProColor = _Product.GetAll().AsQueryable().Include(x => x.ProductColors).FirstOrDefault(x => x.ProductId == id);
                    ViewBag.getProSize = _Product.GetAll().AsQueryable().Include(x => x.ProductSizes).FirstOrDefault(x => x.ProductId == id);
                    ModelState.AddModelError("", "Lỗi trong quá trình sửa");
                    return View(p);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewBag.getCatPro = _Product.GetAll().AsQueryable().Include(x => x.CategoryProducts).FirstOrDefault(x => x.ProductId == id);
                    ViewBag.getProColor = _Product.GetAll().AsQueryable().Include(x => x.ProductColors).FirstOrDefault(x => x.ProductId == id);
                    ViewBag.getProSize = _Product.GetAll().AsQueryable().Include(x => x.ProductSizes).FirstOrDefault(x => x.ProductId == id);
                    return View(p);

                }
                
                
            }
            
            return View(p);
        }
        [CustomAuth(Roles = "DELETE")]
        public ActionResult Delete(int id)
        {

            try
            {
                
                // Phải check trong bảng OrderDetail xem sp đã được mua hay chưa
                if (_od.GetBy(x=> x.ProductId == id).Count() >0)
                {
                    TempData["msg"] = new ResponseMessage()
                    {
                        Type = "alert-danger",
                        Message = "Sản Phẩm Này Đã Được Đặt Hàng"
                    };
                    return RedirectToAction("Index");
                }
                // Xóa danh mục của sản phẩm
                _CategoryProduct.RemoveRange(_CategoryProduct.GetBy(x => x.ProductId == id));
                // xóa thuộc tính của sản phẩm
                _proAtt.RemoveRange(_proAtt.GetBy(x => x.ProductId == id));
                // xóa size của sản phẩm
                _proSize.RemoveRange(_proSize.GetBy(x => x.ProductId == id));
                // xóa color của sản phẩm
                _proColor.RemoveRange(_proColor.GetBy(x => x.ProductId == id));
                //Xóa sp
                _Product.Remove(id);
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