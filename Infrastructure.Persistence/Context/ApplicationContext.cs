using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> dbContext) : base(dbContext) { }

        public DbSet<DefaultProduct> DefaultProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DashBoard> DashBoard { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region "Tables"

            modelBuilder.Entity<DefaultProduct>().ToTable("DefaultProducts");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<DashBoard>().ToTable("DashBoard");



            #endregion

            #region "Primary Keys"

            modelBuilder.Entity<DefaultProduct>().HasKey(d => d.Id);
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<DashBoard>().HasKey(d => d.Id);



            #endregion

            #region "Relations"

            modelBuilder.Entity<DefaultProduct>()
                .HasMany<Product>(d => d.Products)
                .WithOne(p => p.DefaultProduct)
                .HasForeignKey(p => p.DefaultProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany<DefaultProduct>(d => d.DefaultProducts)
                .WithOne(c => c.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region "Properties Config"

            #region Client

            modelBuilder.Entity<Client>().Property(c => c.IdCard)
                .IsRequired(false)
                .HasMaxLength(15);

            modelBuilder.Entity<Client>().Property(c => c.Tel)
                .IsRequired(false)
                .HasMaxLength(13);

            modelBuilder.Entity<Client>().Property(c => c.FullName)
               .IsRequired(true)
               .HasMaxLength(13);

            modelBuilder.Entity<Client>().Property(c => c.Owed)
               .IsRequired(true);

            #endregion

            #region Product

            modelBuilder.Entity<Product>().Property(p => p.SalePrice)
              .IsRequired();

            modelBuilder.Entity<Product>().Property(p => p.PurchasePrice)
             .IsRequired();

            modelBuilder.Entity<Product>().Property(p => p.UserId)
             .IsRequired();

            #endregion

            #region DefaultProduct

            modelBuilder.Entity<DefaultProduct>().Property(d => d.Img).IsRequired(false);

            #endregion

            #region Categories

            modelBuilder.Entity<Category>().Property(n => n.Name)
             .IsRequired();

            modelBuilder.Entity<Category>().Property(n => n.Description)
             .IsRequired(false);

            #endregion

            #region DashBoard

            modelBuilder.Entity<DashBoard>().Property(d => d.UserId).IsRequired();




            #endregion

            #endregion
        }


    }
}
