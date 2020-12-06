using College.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(College.Startup))]
namespace College
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdminAndUserRoles();
        }
        private void CreateAdminAndUserRoles()
        {
            var ctx = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(ctx));

            // adaugam rolurile pe care le poate avea un utilizator
            // din cadrul aplicatiei
            if (!roleManager.RoleExists("Admin"))
            {
                // adaugam rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "sorina@gmail.com";
                user.Email = "sorina@gmail.com";

                var adminCreated = userManager.Create(user, "Admin2020!");

                if (adminCreated.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("User"))
            {
                // adaugati rolul specific aplicatiei voastre
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

                // se adauga utilizatorul
                //var user = new ApplicationUser();
                //user.UserName = "your_user_email";
                //user.Email = "your_user_email";
            }
        }

    }
}
