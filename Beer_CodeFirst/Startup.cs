using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Beer_CodeFirst.Identity;


[assembly: OwinStartup(typeof(Beer_CodeFirst.Startup))]

namespace Beer_CodeFirst
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            this.CreateRolesAndUsers();
        }
        public void CreateRolesAndUsers()
        {
            var roleManager=new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new Identity.AppDbContext()));
            var appDbContext=new Identity.AppDbContext();
            var appuserStore = new AppUserStore(appDbContext);
            var userManager = new UserManager<AppUser>(appuserStore);
            if (!roleManager.RoleExists("Admin"))
            {
                var role= new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

            }
            if (userManager.FindByName("Admin")==null)
            {
                var user = new AppUser();
                user.UserName = "Admin";
                user.Email = "Admin@gmail.com";
                String userpwd="Admin@123";
                var chkUser = userManager.Create(user, userpwd);
                if (chkUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }

            }

            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }
            if (userManager.FindByName("Manager") == null)
            {
                var user = new AppUser();
                user.UserName = "Manager";
                user.Email = "Manager@gmail.com";
                String userpwd = "Manager@123";
                var chkUser = userManager.Create(user, userpwd);
                if (chkUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Manager");
                }

            }

            if (!roleManager.RoleExists("Customer"))
            {
                var role = new IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);

            }

        }
    }
}
