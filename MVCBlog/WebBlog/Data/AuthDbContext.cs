using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebBlog.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed Roles
            var adminRoleId = "585ba892-c7d7-4f14-8c02-1bc85f12177d";
            var superAdminRoleId = "4943b642-f457-4991-993f-4a3dafd997e9";
            var userRoleId = "b32fd0bd-08d1-4601-8da9-6b3e2dff00b1";

            var roles = new List<IdentityRole> {
                new IdentityRole{
                    Name="Admin",
                    NormalizedName="Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp=adminRoleId
                },
                new IdentityRole{
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp=superAdminRoleId
                },
                new IdentityRole{
                    Name="User",
                    NormalizedName="User",
                    Id = userRoleId,
                    ConcurrencyStamp=userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //Seed SuperAdminUser
            var superAdminId = "5b9e9185-1b26-441f-a2ab-62d87a77b86b";
            var superAdminUser = new IdentityUser
            {
                UserName = "superAdmin@blog.com",
                Email = "superAdmin@blog.com",
                NormalizedEmail = "superAdmin@blog.com".ToUpper(),
                NormalizedUserName = "superAdmin@blog.com".ToUpper(),
                Id = superAdminId
            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add All Roles to SuperAdmin
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>{ 
                    RoleId= adminRoleId,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>{
                    RoleId= superAdminRoleId,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>{
                    RoleId= userRoleId,
                    UserId=superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
