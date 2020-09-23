using Project.BAL.Repositories;
using Project.Models.DataMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ProjectGroup4.Controllers
{
    public class ProductsController : Controller
    {
        Repository<Product> _product;
        Repository<Category> _cat;
        Repository<Attributes> _attr;
        Repository<AttributeType> _attrType;
        Repository<ProductAttribute> _proAttr;
        Repository<Size> _size;
        Repository<Color> _color;
        Repository<ProductColor> _proColor;
        Repository<ProductSize> _proSize;

        public ProductsController()
        {
            _product = new Repository<Product>();
            _cat = new Repository<Category>();
            _attr = new Repository<Attributes>();
            _proAttr = new Repository<ProductAttribute>();
            _attrType = new Repository<AttributeType>();
            _size = new Repository<Size>();
            _color = new Repository<Color>();
            _proColor = new Repository<ProductColor>();
            _proSize = new Repository<ProductSize>();
        }
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult ProductDetail(int? id)
        {
            if (id == null || id == 0)
            {
                return View("_404");
            }
            var _p = _product.GetAll().AsQueryable().Include(x => x.ProductAttributes).Include(x=>x.ProductSizes).Include(x=>x.ProductColors).FirstOrDefault(x => x.ProductId == id);

            var data = (
                               from p in _p.ProductAttributes
                               join attr in _attr.GetAll() on p.AttributeId equals attr.AttributeId
                               join t in _attrType.GetAll() on attr.AtttypeId equals t.AtttypeId
                               where t.Important == true
                               group attr by new { t.AtttypeId, t.TypeName } into gr
                               select new
                               {
                                   AtttypeId = gr.Key.AtttypeId,
                                   TypeName = gr.Key.TypeName,
                                   Attributes = gr.ToList()
                               }).Select(x => new AttributeType
                               {
                                   AtttypeId = x.AtttypeId,
                                   TypeName = x.TypeName,
                                   Attributes = x.Attributes
                               }).ToList();
           
            ViewBag.types = data;
            ViewBag.Size = _size.GetAll();
            
            ViewBag.Color = _color.GetAll();
            //_proColor.GetAll().AsQueryable().Include(x => x.ProductId).ToList();
            return View(_p);

        }


        public PartialViewResult ProductView()
        {
            DateTime GetDay = DateTime.Now;
            DateTime SDay = GetDay.AddDays(-7);
            var data = _product.GetBy(x => x.Status == true && x.Quantity > 0).Where(x => SDay < x.CreateDate).Take(12);
            return PartialView("_ProductView", data);
        }
        public PartialViewResult BestSellProduct()
        {
            var data = _product.GetBy(x => x.Status == true && x.Quantity > 0).Where(x => x.Discount > 5).OrderByDescending(x=>x.Discount).Take(10);
            return PartialView("_ProductSell", data);
        }
        public PartialViewResult AllProduct(int page = 1, int pageSize = 8, string search = null)
        {
            var data = _product.GetBy(x => x.Status == true && x.Quantity > 0);
            return PartialView("_AllProduct", data.OrderBy(x => x.ProductId).ToPagedList(page, pageSize));
        }
    }
}