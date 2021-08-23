using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InventMS
{
    public class AppDataContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        public AppDataContext()
        {
        }

        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Manufacturer> Manufacturers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Models.Category>()
            //    .HasMany(c => c.Products)
            //    .WithOne(p => p.Category)
            //    .OnDelete(DeleteBehavior.SetNull);

            //builder.Entity<Models.Manufacturer>()
            //    .HasMany(m => m.Products)
            //    .WithOne(p => p.Manufacturer)
            //    .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            //Console.(sqlString);
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnectionString"));
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
