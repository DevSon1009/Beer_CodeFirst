using Beer_CodeFirst.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beer_CodeFirst.Others; // Thay "Others" bằng tên folder Sơn vừa tạo
using Newtonsoft.Json.Linq;  // Đảm bảo đã cài Newtonsoft.Json từ NuGet
namespace Beer_CodeFirst.Controllers
{
    public class CartController : Controller
    {
        // Hàm lấy giỏ hàng hiện tại từ Session
        private CompanyDBContext db = new CompanyDBContext();

        //public object MoMoSecurity { get; private set; }

        public List<CartItem> GetCart()
        {
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            Session["Cart"] = cart;
            return cart;
        }

        public ActionResult AddToCart(int id)
        {
            var cart = GetCart();
            var product = db.Products.Find(id);

            if (product != null)
            {
                var item = cart.FirstOrDefault(p => p.ProductId == id);
                if (item == null)
                {
                    cart.Add(new CartItem
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Price = (decimal)product.Price, // Sửa lỗi gạch đỏ tại đây
                        ImageUrl = product.ImageUrl,
                        Quantity = 1
                    });
                }
                else { item.Quantity++; }
            }
            return RedirectToAction("Index");
        }

        // --- CHỨC NĂNG THANH TOÁN ---
        [HttpPost]
        public ActionResult ProcessOrder(string shippingAddress, string phone)
        {
            var cart = GetCart();
            if (cart.Count == 0) return RedirectToAction("Index", "Home");

            // 1. Tạo đơn hàng mới (bảng Orders)
            var order = new Order
            {
                OrderDate = DateTime.Now,
                ShippingAddress = shippingAddress,
                Phone = phone,
                TotalAmount = (decimal)(double)cart.Sum(x => x.Price * x.Quantity),
                Status = "Pending"
            };

            db.Orders.Add(order);
            db.SaveChanges(); // Lưu để lấy được OrderId

            // 2. Lưu chi tiết từng món (bảng OrderItems)
            foreach (var item in cart)
            {
                var detail = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PriceAtOrder = (decimal)(double)item.Price
                };
                db.OrderItems.Add(detail);
            }

            db.SaveChanges();
            Session["Cart"] = null; // Xóa giỏ hàng sau khi xong

            return View("OrderSuccess");
        }
        public ActionResult Index()
        {
            var cart = GetCart();
            return View(cart); // Máy sẽ tìm file Views/Cart/Index.cshtml để hiện ra
        }
        public ActionResult UpdateQuantity(int id, int amount)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity += amount;
                if (item.Quantity <= 0) cart.Remove(item); // Nếu giảm về 0 thì xóa luôn
            }
            return RedirectToAction("Index");
        }

        // 2. Xóa hẳn sản phẩm khỏi giỏ
        public ActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item != null) cart.Remove(item);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Payment()
        {
            var cart = GetCart();
            if (cart.Count == 0) return RedirectToAction("Index");

            // Tạo đơn hàng – chỉ dùng các thuộc tính có trong model Order
            var order = new Order
            {
                // Nếu chưa có UserId, tạm thời gán 0 hoặc 1 (sau sẽ cập nhật)
                UserId = 0, // Bạn có thể lấy từ Session hoặc User.Identity nếu đã đăng nhập
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(x => x.Price * x.Quantity),
                // Các trường ShippingAddress, Phone, Note có thể để null hoặc lấy từ form nếu có
                ShippingAddress = null,
                Phone = null,
                Note = null,
                Status = "Pending",
                PaymentMethod = "MoMo",
                PaymentStatus = "Pending",
                TransactionId = null,
                PaymentDate = null
            };

            db.Orders.Add(order);
            db.SaveChanges(); // Lưu để lấy OrderId

            foreach (var item in cart)
            {
                db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PriceAtOrder = item.Price,
                    CreatedDate = DateTime.Now   // OrderItem có CreatedDate
                });
            }
            db.SaveChanges();

            Session["Cart"] = null;
            TempData["OrderSuccess"] = true;
            TempData["TotalAmount"] = order.TotalAmount;
            return RedirectToAction("OrderSuccess");
        }
        // Action này MoMo sẽ gọi đến sau khi người dùng thanh toán xong (ReturnUrl)
        public ActionResult ConfirmPaymentClient(string errorCode, string message)
        {
            if (errorCode == "0")
            {
                // Thanh toán thành công
                ViewBag.Message = "Thanh toán MoMo thành công! Cạn ly thôi Sơn ơi!";
                Session["Cart"] = null; // Xóa giỏ hàng sau khi mua xong
            }
            else
            {
                // Thất bại
                ViewBag.Message = "Thanh toán thất bại: " + message;
            }
            return View();
        }
        public ActionResult OrderSuccess()
        {
            ViewBag.Message = "Thanh toán MoMo thành công (Test mode)!";
            return View();
        }
    }
}