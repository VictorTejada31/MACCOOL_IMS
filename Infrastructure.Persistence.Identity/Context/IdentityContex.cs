using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Context
{
    public class IdentityContex : IdentityDbContext<ApplicationUser>
    {
        public IdentityContex(DbContextOptions<IdentityContex> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");

            #region Tables

            builder.Entity<ApplicationUser>(options =>
            {
                options.ToTable("Users");
            });

            builder.Entity<IdentityRole>(options =>
            {
                options.ToTable("Roles");
            });

            builder.Entity<IdentityUserLogin<string>>(options =>
            {
                options.ToTable("UserLogin");
            });

            builder.Entity<IdentityUserRole<string>>(options =>
            {
                options.ToTable("UserRole");
            });

            builder.Entity<IdentityRoleClaim<string>>(options =>
            {
                options.ToTable("RoleClaim");
            });

            builder.Entity<IdentityUserClaim<string>>(options =>
            {
                options.ToTable("UserClaim");
            });

            builder.Entity<IdentityUserToken<string>>(options =>
            {
                options.ToTable("UserToken");
            });


            #endregion
            #region Property Conf. ApplicationUser 

            builder.Entity<ApplicationUser>(options =>
            {
                options.Property(user => user.CretedBy).IsRequired(false);
                options.Property(user => user.FirstName).IsRequired(true).HasMaxLength(70);
                options.Property(user => user.LastName).IsRequired(true).HasMaxLength(70);
                options.Property(user => user.isOnline).IsRequired(true);
                options.Property(user => user.LastConnection).IsRequired(true);
            });

            #endregion




        }
    }
}
