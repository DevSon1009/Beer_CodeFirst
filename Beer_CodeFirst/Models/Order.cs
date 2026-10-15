using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Beer_CodeFirst.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }

        public string Status { get; set; } = "Pending";           // Trạng thái đơn: Pending, Confirmed, Shipping, Delivered, Cancelled
        public string PaymentMethod { get; set; } = "MoMo";       // MoMo, COD,...

        // === Phần quan trọng cho thanh toán MoMo ===
        public string PaymentStatus { get; set; } = "Pending";    // Pending, Success, Failed, Cancelled
        public string TransactionId { get; set; }                 // Mã giao dịch từ MoMo
        public DateTime? PaymentDate { get; set; }                // Ngày thanh toán thành công

        public ICollection<OrderItem> OrderItems { get; set; }

        // Navigation (tùy chọn)
        //[ForeignKey("UserId")]
        //public virtual ApplicationUser User { get; set; }   // nếu bạn có class ApplicationUser kế thừa IdentityUser
    }
}