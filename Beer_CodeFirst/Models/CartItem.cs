using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 

namespace Beer_CodeFirst.Models
{
    public class CartItem
    {
        [Key]
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public string ImageUrl { get; set; }
            public decimal Total => Price * Quantity;
        
    }
}