using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beer_CodeFirst.Identity;
using Beer_CodeFirst.Models;
using Beer_CodeFirst.ModelView;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Beer_CodeFirst.Controllers
{
    public class AccountController : Controller
    {
        // GET: Acount
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM rvm)
        {
            if (ModelState.IsValid)
            {
                var appDbcontext = new AppDbContext();
                var userStore = new AppUserStore(appDbcontext);
                var userManager = new UserManager<AppUser>(userStore);
                var passwdHash = Crypto.HashPassword(rvm.Password);
                var user = new AppUser()
                {
                    UserName = rvm.UserName,
                    Email = rvm.Email,
                    PasswordHash = passwdHash

                };

                IdentityResult result = userManager.Create(user);
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Customer");
                    var authenManager = HttpContext.GetOwinContext().Authentication;

                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);


                }
                return RedirectToAction("Index", "Home");


            }
            else
            {
                ModelState.AddModelError("error", "error");
            }
            return View();



        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LogionVM lvm)
        {
            var appDbcontext = new AppDbContext();
            var userStore = new AppUserStore(appDbcontext);
            var userManager = new UserManager<AppUser>(userStore);
            var user = userManager.Find(lvm.UserName, lvm.Password);
            if (user != null)
            {

                var authenManager = HttpContext.GetOwinContext().Authentication;

                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("error", "Invalid username or password");
                return View();
            }



        }
        public ActionResult Logout()
        {
            var authenManager = HttpContext.GetOwinContext().Authentication;
            authenManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}