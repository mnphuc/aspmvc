using PagedList;
using Project.BAL.Repositories;
using Project.Models.DataMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ProjectGroup4.Controllers
{
    public class CategoriesController : Controller
    {
        Repository<Category> _cat;
        Repository<Provider> _Provider;
        Repository<Size> _Size;
        Repository<Color> _Color;
        Repository<Product> _Product;
        Repository<CategoryProduct> _CatPro;
        Repository<ProductSize> _SizePro;
        Repository<ProductColor> _ColorPro;

        public CategoriesController()
        {
            _cat = new Repository<Category>();
            _Provider = new Repository<Provider>();
            _Size = new Repository<Size>();
            _Color = new Repository<Color>();
            _Product = new Repository<Product>();
            _CatPro = new Repository<CategoryProduct>();
            _SizePro = new Repository<ProductSize>();
            _ColorPro = new Repository<ProductColor>();

        }
        // GET: Categories
        public ActionResult Category(int? page,int pageSize=5 )
        {
            int pageN = (page ?? 1);
            return View(_Product.GetAll().OrderBy(x=>x.CreateDate).ToPagedList(pageN, pageSize));
        }

        public PartialViewResult CategoryView()
        {
            var data = _cat.GetBy(x => x.ShowHome == true);

            return PartialView("_CategoryView", data);
        }

        public PartialViewResult BrandView()
        {
            var data = _Provider.GetBy(x => x.Status == true);

            return PartialView("_BrandView", data);
        }

        public PartialViewResult SizeView()
        {
            var data = _Size.GetAll();

            return PartialView("_SizeView", data);
        }

        public PartialViewResult ColorView()
        {
            var data = _Color.GetAll();

            return PartialView("_ColorView", data);
        }

        public PartialViewResult DiscountView()
        {
            var data = _Product.GetBy(x => x.Status == true).OrderByDescending(x=>x.Discount).Select(x => x.Discount).Distinct();

            return PartialView("_DiscountView", data);
        }

        public PartialViewResult ProductCategory(string catId, string keyw, int? page, int pageSize = 9)
        {
            if (!String.IsNullOrEmpty(keyw))
            {
                int pagez = (page ?? 1);
                return PartialView("_ProductCategoryAll", _Product.GetAll().Where(s => s.ProductName.ToLower().Contains(keyw.ToLower())).OrderBy(x => x.CreateDate).ToPagedList(pagez, pageSize));
            }
            if (String.IsNullOrEmpty(catId))
            {
                int pagex = (page ?? 1);
                return PartialView("_ProductCategoryAll", _Product.GetAll().OrderBy(x => x.CreateDate).ToPagedList(pagex, pageSize));
            }
            var data = _CatPro.GetAll().AsQueryable().Include(x => x.Product).ToList();
            if (!String.IsNullOrEmpty(catId))
            {
                var catIds = catId.Split(',');
                List<CategoryProduct> lst = new List<CategoryProduct>();
                foreach (var item in catIds)
                {
                    lst.AddRange(data.Where(x => x.CategoryId == Convert.ToInt32(item)));
                }
                data = lst;
            }
            return PartialView("_ProductCategory", data);
        }


        public PartialViewResult BrandCategory(int providerId)
        {
            var data = _Product.GetAll().AsQueryable().Include(x => x.Provider).ToList();
            List<Product> lst = new List<Product>();
            lst.AddRange(data.Where(x => x.ProviderId == providerId));
            data = lst;
            return PartialView("_BrandCategory", data);
        }


        public PartialViewResult SizeCategory(int SizeId)
        {
            var data = _SizePro.GetAll().AsQueryable().Include(x => x.Product).ToList();
            List<ProductSize> lst = new List<ProductSize>();

            lst.AddRange(data.Where(x => x.SizeId == SizeId));


            data = lst;
            return PartialView("_SizeCategory", data);
        }

        public PartialViewResult ColorCategory(int ColorId)
        {
            var data = _ColorPro.GetAll().AsQueryable().Include(x => x.Product).ToList();
            List<ProductColor> lst = new List<ProductColor>();
            lst.AddRange(data.Where(x => x.ColorId == ColorId));
            data = lst;
            return PartialView("_ColorCategory", data);
        }

        public PartialViewResult DiscountCategory(int Discount)
        {
            var data = _Product.GetAll().AsQueryable().ToList();
            List<Product> lst = new List<Product>();
            lst.AddRange(data.Where(x => x.Discount == Discount));
            data = lst;
            return PartialView("_DiscountCategory", data);
        }
    }
}