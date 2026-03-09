using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beer_CodeFirst.Filters;

namespace Beer_CodeFirst.Areas.Admin.Controllers
{
        [AdminAuthorization]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}