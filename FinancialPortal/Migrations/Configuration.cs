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

            if (!context.Roles.Any(r => r.Name == HouseholdRoleName.HEAD_OF_HOUSEHOLD))
            {
                applicationRoleManager.Create(new IdentityRole { Name = HouseholdRoleName.HEAD_OF_HOUSEHOLD });
            }

            if (!context.Roles.Any(r => r.Name == HouseholdRoleName.MEMBER))
            {
                applicationRoleManager.Create(new IdentityRole { Name = HouseholdRoleName.MEMBER });
            }

            if (!context.Roles.Any(r => r.Name == HouseholdRoleName.NONE))
            {
                applicationRoleManager.Create(new IdentityRole { Name = HouseholdRoleName.NONE });
            }

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "samuel_smith@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "samuel_smith@mailinator.com",
                    Email = "samuel_smith@mailinator.com",
                    FirstName = "Samuel",
                    LastName = "Smith",
                }, "CoderFoundry1!");
            }

            string administratorUserID = userManager.FindByEmail("samuel_smith@mailinator.com").Id;
            userManager.AddToRole(administratorUserID, HouseholdRoleName.HEAD_OF_HOUSEHOLD);
            userManager.AddToRole(administratorUserID, HouseholdRoleName.MEMBER);
            userManager.AddToRole(administratorUserID, HouseholdRoleName.NONE);
        }
    }
}