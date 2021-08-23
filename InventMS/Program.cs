using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;

namespace InventMS
{
    public class Program
    {
        private static void SeedDatabase(AppDataContext db)
        {
            var category = new Models.Category { Name = "Electronic" };
            db.Add(category);
            db.SaveChanges();

            var manufacturer = new Models.Manufacturer { Name = "Microsoft", Country = "USA" };
            db.Add(manufacturer);
            db.SaveChanges();

            var product = new Models.Product { Name = "Xbox One", MSRPrice = 299.99f, CategoryId = category.Id, ManufacturerId = manufacturer.Id, Quantity = 1, CustomerRating = 4.5f };
            db.Add(product);
            db.SaveChanges();
        }
        public static void Main(string[] args)
        {
            AppDataContext db = new AppDataContext();

            var dataAccess = new DataAccessProvider(db);
            var products = dataAccess.GetProductsData();

            if (products.Count == 0)
            {
                SeedDatabase(db); //Seed db with sample records if table is empty
            }
            
            
            CreateHostBuilder(args).Build().Run();
        }
       
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
