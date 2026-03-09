using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Beer_CodeFirst.Models
{
    public class CompanyDBContext : DbContext
    {
        internal object categories;

        public CompanyDBContext() : base("BeerString") { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}