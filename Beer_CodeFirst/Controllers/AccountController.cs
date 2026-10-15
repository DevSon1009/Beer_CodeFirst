using Beer_CodeFirst.Identity;
using Beer_CodeFirst.Models;
using Beer_CodeFirst.ModelView;
using Beer_CodeFirst.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Beer_CodeFirst.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM rvm)
        {
            if (ModelState.IsValid)
            {
                var appDbContext = new AppDbContext();
                var userStore = new AppUserStore(appDbContext);
                var userManager = new UserManager<AppUser>(userStore);

                // Mã hóa mật khẩu
                var passwordHash = Crypto.HashPassword(rvm.Password);

                // Tạo token xác nhận
                var confirmationToken = TokenService.GenerateEmailToken(rvm.Email);
                var expiryDate = DateTime.Now.AddHours(24);

                // Tạo user mới
                var user = new AppUser()
                {
                    UserName = rvm.UserName,
                    Email = rvm.Email,
                    PasswordHash = passwordHash,
                    IsEmailConfirmed = false,
                    EmailConfirmationToken = confirmationToken,
                    TokenExpiryDate = expiryDate,
                    CreatedAt = DateTime.Now
                };

                // Tạo user trong database
                IdentityResult result = userManager.Create(user);

                if (result.Succeeded)
                {
                    // Thêm role Customer
                    userManager.AddToRole(user.Id, "Customer");

                    // Tạo link xác nhận
                    var confirmationLink = TokenService.GenerateConfirmationLink(confirmationToken, user.Email);

                    // Gửi email xác nhận
                    EmailService.SendConfirmationEmail(user.Email, confirmationLink);

                    // Lưu email vào TempData để dùng cho view
                    TempData["Email"] = user.Email;
                    TempData["Message"] = "Vui lòng kiểm tra email để xác nhận tài khoản!";

                    return RedirectToAction("RegistrationConfirmation");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(rvm);
        }

        // GET: RegistrationConfirmation
        public ActionResult RegistrationConfirmation()
        {
            ViewBag.Message = TempData["Message"] ?? "Email xác nhận đã được gửi!";
            ViewBag.Email = TempData["Email"];
            return View();
        }

        // GET: ConfirmEmail
        public ActionResult ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Link xác nhận không hợp lệ!";
                return View("EmailConfirmationResult");
            }

            var appDbContext = new AppDbContext();
            var userStore = new AppUserStore(appDbContext);
            var userManager = new UserManager<AppUser>(userStore);
            var user = userManager.FindByEmail(email);

            if (user == null)
            {
                ViewBag.Error = "Không tìm thấy tài khoản!";
                return View("EmailConfirmationResult");
            }

            if (user.IsEmailConfirmed)
            {
                ViewBag.Message = "Tài khoản đã được xác nhận trước đó!";
                return View("EmailConfirmationResult");
            }

            if (user.EmailConfirmationToken != token)
            {
                ViewBag.Error = "Mã xác nhận không hợp lệ!";
                return View("EmailConfirmationResult");
            }

            if (user.TokenExpiryDate < DateTime.Now)
            {
                ViewBag.Error = "Link xác nhận đã hết hạn (24 giờ)! Vui lòng đăng ký lại.";
                return View("EmailConfirmationResult");
            }

            // Xác nhận thành công
            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.TokenExpiryDate = null;
            userManager.Update(user);

            ViewBag.Message = "Xác nhận email thành công! Bạn có thể đăng nhập ngay.";
            return View("EmailConfirmationResult");
        }

        // POST: ResendConfirmation
        [HttpPost]
        public JsonResult ResendConfirmation(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "Email không hợp lệ!" });
            }

            var appDbContext = new AppDbContext();
            var userStore = new AppUserStore(appDbContext);
            var userManager = new UserManager<AppUser>(userStore);
            var user = userManager.FindByEmail(email);

            if (user == null)
            {
                return Json(new { success = false, message = "Email không tồn tại trong hệ thống!" });
            }

            if (user.IsEmailConfirmed)
            {
                return Json(new { success = false, message = "Tài khoản đã được xác nhận trước đó!" });
            }

            // Tạo token mới
            var newToken = TokenService.GenerateEmailToken(email);
            user.EmailConfirmationToken = newToken;
            user.TokenExpiryDate = DateTime.Now.AddHours(24);
            userManager.Update(user);

            // Gửi lại email
            var confirmationLink = TokenService.GenerateConfirmationLink(newToken, email);
            EmailService.SendConfirmationEmail(email, confirmationLink);

            return Json(new { success = true, message = "Email xác nhận đã được gửi lại! Vui lòng kiểm tra hộp thư." });
        }

        // GET: Login
        public ActionResult Login()
        {
            ModelState.Clear();
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogionVM lvm)
        {
            if (!ModelState.IsValid)
            {
                return View(lvm);
            }

            var appDbContext = new AppDbContext();
            var userStore = new AppUserStore(appDbContext);
            var userManager = new UserManager<AppUser>(userStore);
            var user = userManager.Find(lvm.UserName, lvm.Password);

            if (user != null)
            {
                // ✅ CHỈ KIỂM TRA XÁC NHẬN EMAIL NẾU KHÔNG PHẢI ADMIN
                var isAdmin = userManager.IsInRole(user.Id, "Admin");

                if (!isAdmin && !user.IsEmailConfirmed)
                {
                    ModelState.AddModelError("", "Tài khoản chưa được xác nhận! Vui lòng kiểm tra email để xác nhận tài khoản.");
                    return View(lvm);
                }

                var authenManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

                if (isAdmin)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng!");
                return View(lvm);
            }
        }

        // GET: Logout
        public ActionResult Logout()
        {
            var authenManager = HttpContext.GetOwinContext().Authentication;
            authenManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET: Profile
        public ActionResult Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            string username = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new Profile
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };

            return View(model);
        }

        // POST: Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(Profile model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.UserId);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.PhoneNumber = model.Phone;
                    db.SaveChanges();
                    ViewBag.Message = "Cập nhật thông tin thành công!";
                }
            }
            return View(model);
        }
    }
}