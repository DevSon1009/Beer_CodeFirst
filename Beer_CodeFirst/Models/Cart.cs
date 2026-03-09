using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Beer_CodeFirst.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Status { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    
}
}