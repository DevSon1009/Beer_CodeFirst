using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Beer_CodeFirst.Identity
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext() : base("BeerString")
        {
        }
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}