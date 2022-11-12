using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace Mock3.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Mock3.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Mock3.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Roles.Any())
            {
                context.Roles.AddOrUpdate(
                    new IdentityRole { Name = "User" }
                    , new IdentityRole { Name = "Admin" });
            }
        }
    }
}
