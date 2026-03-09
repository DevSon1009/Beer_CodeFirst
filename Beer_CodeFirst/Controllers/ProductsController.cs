using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beer_CodeFirst.Models;
using System.Data.Entity;


namespace Beer_CodeFirst.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {

            CompanyDBContext db = new CompanyDBContext();

            var products = db.Products
                .Include(p => p.Category)
                .ToList();

            return View(products);
        }
    }
}