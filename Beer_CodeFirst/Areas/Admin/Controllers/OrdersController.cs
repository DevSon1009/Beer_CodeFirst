using Beer_CodeFirst.Filters;
using Beer_CodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beer_CodeFirst.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class OrdersController : Controller
    {
        private readonly CompanyDBContext db = new CompanyDBContext();

        // Danh sách đơn hàng + lọc
        public ActionResult Index(string paymentStatus = "all")
        {
            var orders = db.Orders
                           .Include("OrderItems")
                           .OrderByDescending(o => o.OrderDate)
                           .AsQueryable();

            if (paymentStatus != "all" && !string.IsNullOrEmpty(paymentStatus))
            {
                orders = orders.Where(o => o.PaymentStatus == paymentStatus);
            }

            ViewBag.CurrentFilter = paymentStatus;

            ViewBag.PaymentStatuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "all", Text = "Tất cả đơn hàng" },
                new SelectListItem { Value = "Pending", Text = "Chưa thanh toán" },
                new SelectListItem { Value = "Success", Text = "Thanh toán thành công" },
                new SelectListItem { Value = "Cancelled", Text = "Đã hủy thanh toán" },
                new SelectListItem { Value = "Failed", Text = "Thanh toán thất bại" }
            };

            return View(orders.ToList());
        }

        // Chi tiết đơn hàng
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var order = db.Orders
                          .Include("OrderItems")
                          .FirstOrDefault(o => o.OrderId == id.Value);

            if (order == null)
            {
                TempData["Error"] = $"Không tìm thấy đơn hàng #{id}";
                return RedirectToAction("Index");
            }

            return View(order);
        }
    }
}