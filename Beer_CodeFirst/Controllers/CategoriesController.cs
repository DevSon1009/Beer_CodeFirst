using Beer_CodeFirst.Filters;
using Beer_CodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beer_CodeFirst.Controllers
{

    [MyAuthenFilter]
    public class CategoriesController : Controller
    {
        // GET: Categories
        public ActionResult Index()
        {
            CompanyDBContext db = new CompanyDBContext();
            List<Category> category = db.Categories.ToList();

            return View(category);
        }
    }
}