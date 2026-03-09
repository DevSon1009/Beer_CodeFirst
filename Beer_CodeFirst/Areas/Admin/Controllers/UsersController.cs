using Beer_CodeFirst.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beer_CodeFirst.Areas.Admin.Controllers
{
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