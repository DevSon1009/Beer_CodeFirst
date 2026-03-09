using Beer_CodeFirst.Filters;
using Beer_CodeFirst.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beer_CodeFirst.Filters;

namespace Beer_CodeFirst.Areas.Admin.Controllers
{
        [AdminAuthorization]
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
            AppDbContext db = new AppDbContext();
                var users = db.Users.ToList();

            return View(users);
        }
    }
}