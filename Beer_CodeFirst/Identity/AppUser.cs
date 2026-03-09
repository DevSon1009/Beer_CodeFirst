using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;


namespace Beer_CodeFirst.Identity
{
    public class AppUser : IdentityUser
    {
        
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }

    }
}