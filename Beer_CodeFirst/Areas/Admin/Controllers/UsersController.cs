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




        public ActionResult Delete(string id)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(id);

                if (user.UserName == "Admin")
                {
                    TempData["Error"] = "Không thể xóa tài khoản Admin!";
                    return RedirectToAction("Index");
                }

                db.Users.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }




}