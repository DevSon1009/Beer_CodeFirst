using Beer_CodeFirst.Models;
using Beer_CodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
//using System.Windows.Controls;
using System.Data.Entity;

namespace Beer_CodeFirst.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(
    string search = "",
    string sortColumn = "ProductName",
    string iconClass = "fa-sort-asc",
    int page = 1)
        {
            CompanyDBContext db = new CompanyDBContext();

            var Products = db.Products
                .Where(p => p.ProductName.Contains(search))
                .ToList();

            // ===== SORT =====
            if (iconClass == "fa-sort-asc")
            {
                Products = Products
                    .OrderBy(p => p.GetType().GetProperty(sortColumn).GetValue(p))
                    .ToList();
                iconClass = "fa-sort-desc";
            }
            else
            {
                Products = Products
                    .OrderByDescending(p => p.GetType().GetProperty(sortColumn).GetValue(p))
                    .ToList();
                iconClass = "fa-sort-asc";
            }

            // ===== PHÂN TRANG =====
            int pageSize = 6;
            int noOfPages = (int)Math.Ceiling((double)Products.Count() / pageSize);

            int skip = (page - 1) * pageSize;

            Products = Products
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            // ===== VIEWBAG =====
            ViewBag.Page = page;
            ViewBag.NoOfPages = noOfPages;
            ViewBag.SortColumn = sortColumn;
            ViewBag.IconClass = iconClass;
            ViewBag.Search = search;

            return View(Products);
        }
        public ActionResult Detail(int? id)
        {
            CompanyDBContext db = new CompanyDBContext();
            Product pro = db.Products.Where(row => row.ProductId == id).FirstOrDefault();
            return View(pro);

        }
        public ActionResult Create()
        {
            CompanyDBContext db = new CompanyDBContext();
            ViewBag.CategoryList = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            CompanyDBContext db = new CompanyDBContext();
            if (!ModelState.IsValid)
            {
                return Content("Model lỗi");
            }

            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //private ActionResult RedỉectToAction(string v)
        //{
        //    throw new NotImplementedException();
        //}

        public ActionResult Edit(int id)
        {
            CompanyDBContext db = new CompanyDBContext();
            Product product = db.Products.Where(row => row.ProductId == id).FirstOrDefault();

            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product pro)
        {
            CompanyDBContext db = new CompanyDBContext();
            Product product = db.Products.Where(row => row.ProductId == pro.ProductId).FirstOrDefault();
            product.ProductName = pro.ProductName;
            product.ProductId = pro.ProductId;
            product.Price = pro.Price;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            CompanyDBContext db = new CompanyDBContext();

            Product product = db.Products.FirstOrDefault(row => row.ProductId == id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }
        [HttpPost]

        public ActionResult Delete(int id, Product P)
        {
            CompanyDBContext db = new CompanyDBContext();
            Product product = db.Products.Where(row => row.ProductId == id).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
