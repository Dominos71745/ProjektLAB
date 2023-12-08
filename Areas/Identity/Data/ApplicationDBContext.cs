using ProjektLAB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektLAB.Areas.Identity.Data;
using static ProjektLAB.Models.Dane;

namespace ProjektLAB.Areas.Identity.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Cars> Cars { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Orders> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Cars>()
          .HasOne(c => c.Category)
          .WithMany(v => v.Cars)
          .HasForeignKey(c => c.CategoryId);


            builder.Entity<Orders>()
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId);

            builder.Entity<Orders>()
                .HasOne(o => o.Car)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CarId);

            // Dodanie klucza obcego dla Cars i Clients
            builder.Entity<Cars>().HasKey(c => new { c.CarId });
            builder.Entity<Clients>().HasKey(c => new { c.ClientId });
        }

        private class ApplicationUserEntityConfiguration :
            IEntityTypeConfiguration<ApplicationUser>
        {
            public void Configure(EntityTypeBuilder<ApplicationUser> builder)
            {
                builder.Property(x => x.FirstName).HasMaxLength(255);
                builder.Property(x => x.LastName).HasMaxLength(255);
            }
        }
    }
}