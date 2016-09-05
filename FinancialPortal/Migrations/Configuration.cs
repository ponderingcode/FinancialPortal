using FinancialPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FinancialPortal.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            RoleManager<IdentityRole> applicationRoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == ApplicationRole.ADMINISTRATOR))
            {
                applicationRoleManager.Create(new IdentityRole { Name = ApplicationRole.ADMINISTRATOR });
            }

            if (!context.Roles.Any(r => r.Name == ApplicationRole.PRIVILEGED_USER))
            {
                applicationRoleManager.Create(new IdentityRole { Name = ApplicationRole.PRIVILEGED_USER });
            }

            if (!context.Roles.Any(r => r.Name == ApplicationRole.STANDARD_USER))
            {
                applicationRoleManager.Create(new IdentityRole { Name = ApplicationRole.STANDARD_USER });
            }

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "dev@ponderingcode.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "dev@ponderingcode.com",
                    Email = "dev@ponderingcode.com",
                    FirstName = "Ryan",
                    LastName = "Fleming",
                }, "CoderFoundry");
            }

            string administratorUserID = userManager.FindByEmail("dev@ponderingcode.com").Id;
            userManager.AddToRole(administratorUserID, ApplicationRole.ADMINISTRATOR);
            userManager.AddToRole(administratorUserID, ApplicationRole.PRIVILEGED_USER);
            userManager.AddToRole(administratorUserID, ApplicationRole.STANDARD_USER);
        }
    }
}