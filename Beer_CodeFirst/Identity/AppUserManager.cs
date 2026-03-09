using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Beer_CodeFirst.Identity
{
   
        public class AppUserManager : UserManager<AppUser>
        {
            public AppUserManager(IUserStore<AppUser> store)
                : base(store)
            {
            }

            public static AppUserManager Create()
            {
                var manager = new AppUserManager(new AppUserStore(new AppDbContext()));

                var provider = new DpapiDataProtectionProvider("BeerShop");
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<AppUser>(provider.Create("EmailConfirmation"));

                return manager;
            }
        }

    }
