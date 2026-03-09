using Beer_CodeFirst.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Beer_CodeFirst.Areas.Admin.Controllers
{
        [AdminAuthorization]
    public class AccountController : Controller
    {
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", new { area = "" });
        }
    }
}