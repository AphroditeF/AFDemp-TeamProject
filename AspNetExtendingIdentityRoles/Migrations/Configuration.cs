using AspNetExtendingIdentityRoles.Models;
using System.Data.Entity.Migrations;

namespace AspNetExtendingIdentityRoles.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        protected override void Seed(ApplicationDbContext context)
        {
            this.AddUserAndRoles();
        }


        bool AddUserAndRoles()
        {
            bool success = false;

            var idManager = new IdentityManager();
            success = idManager.CreateRole("Admin", "Global Access");
            if (!success == true) return success;

            success = idManager.CreateRole("Moderator", "Edit existing records");
            if (!success == true) return success;

            success = idManager.CreateRole("User", "Send messages");
            if (!success) return success;


            var newUser = new ApplicationUser()
            {
                UserName = "leonidas",
                FirstName = "Leonidas",
                LastName = "Guri",
                Email = "leonidasguri@hotmail.com"
            };

            // Be careful here - you  will need to use a password which will 
            // be valid under the password rules for the application, 
            // or the process will abort:
            success = idManager.CreateUser(newUser, "12345abcd");
            if (!success) return success;

            success = idManager.AddUserToRole(newUser.Id, "Admin");
            if (!success) return success;

            success = idManager.AddUserToRole(newUser.Id, "Moderator");
            if (!success) return success;

            success = idManager.AddUserToRole(newUser.Id, "User");
            if (!success) return success;

            return success;
        }
    }
}
