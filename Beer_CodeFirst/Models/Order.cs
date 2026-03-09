using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Beer_CodeFirst.Models
{
    public class Order
    {

        [Key]
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string ShippingAddress { get; set; }

        public string Phone { get; set; }

        public string Note { get; set; }

        public string Status { get; set; }

        public string PaymentMethod { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}