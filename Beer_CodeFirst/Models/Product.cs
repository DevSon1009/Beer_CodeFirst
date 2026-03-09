using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Beer_CodeFirst.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Display(Name ="Product Name")]
        [Required]

        public int CategoryId { get; set; }

        public string ProductName { get; set; }
        [Display(Name = "Price (USD)")]
        [Required]

        public decimal? Price { get; set; }

        public decimal? OriginalPrice { get; set; }

        public double? AlcoholContent { get; set; }

        public int? Volume { get; set; }

        public int? StockQuantity { get; set; }

        public string Origin { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }

      
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}