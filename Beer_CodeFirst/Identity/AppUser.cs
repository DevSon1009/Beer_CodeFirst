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
        public bool IsEmailConfirmed { get; set; } = false;
        public string EmailConfirmationToken { get; set; }
        public DateTime? TokenExpiryDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Email {  get; set; }

    }
}