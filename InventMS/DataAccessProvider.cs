using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InventMS
{
    public class DataAccessProvider : IDataAccessProvider
    {
        private readonly AppDataContext _context;
        public DataAccessProvider(AppDataContext context)
        {
            _context = context;
        }

        // Operations to get data directly (sync)
        public Models.Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }
        public List<Models.Product> GetProductsData()
        {
            return _context.Products.ToList();
        }
        public Models.Manufacturer GetManufacturerById(int id)
        {
            return _context.Manufacturers.Find(id);
        }
        public Models.Product GetProductById(int id)
        {
            return _context.Products.Find(id);
        }


        // Operations using Tasks (async)
        public async Task<Models.Product> GetProductTask(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Models.Manufacturer> GetManufacturerTask(int id)
        {
            return await _context.Manufacturers.FindAsync(id);
        }

        public async Task<IEnumerable<Models.Manufacturer>> GetManufacturersTask()
        {
            return await _context.Manufacturers.ToListAsync();
        }


        public async Task<Models.Product> UpdateProductTask(Models.Product productInput)
        {
            var productNeedUpdate = await _context.Products.FindAsync(productInput.Id);
            if (productNeedUpdate != null)
            {
                productNeedUpdate.Name = productInput.Name;
                productNeedUpdate.MSRPrice = productInput.MSRPrice;
                productNeedUpdate.CustomerRating = productInput.CustomerRating;
                productNeedUpdate.ManufacturerId = productInput.ManufacturerId;
                productNeedUpdate.Quantity = productInput.Quantity;
                productNeedUpdate.CategoryId = productInput.CategoryId;
                productNeedUpdate.Category = _context.Categories.Find(productInput.CategoryId);
                productNeedUpdate.Manufacturer = _context.Manufacturers.Find(productInput.ManufacturerId);

                await _context.SaveChangesAsync();

                return productNeedUpdate;
            }
            return null;
        }

        public async Task<Models.Manufacturer> UpdateManufacturerTask(Models.Manufacturer manufacturerInput)
        {
            var manufacturerNeedUpdate = await _context.Manufacturers.FindAsync(manufacturerInput.Id);
            if (manufacturerNeedUpdate != null)
            {
                manufacturerNeedUpdate.Name = manufacturerInput.Name;
                manufacturerNeedUpdate.Country = manufacturerInput.Country;

                await _context.SaveChangesAsync();
                return manufacturerNeedUpdate;
            }
            return null;
        }

        public async Task<Models.Product> AddProductTask(Models.Product productInput)
        {
            var result = await _context.Products.AddAsync(productInput);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Models.Manufacturer> AddManufacturerTask(Models.Manufacturer manufacturerInput)
        {
            var result = await _context.Manufacturers.AddAsync(manufacturerInput);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<string> DeleteProductTask(int id)
        {
            var productDeleting = await _context.Products.FindAsync(id);
            int idDeleted = productDeleting.Id;
            _context.Products.Remove(productDeleting);
            await _context.SaveChangesAsync();
            return "Deleted product with ID: " + idDeleted;
        }

        public async Task<string> DeleteManufacturerTask(int id)
        {
            var manufacturerDeleting = await _context.Manufacturers.FindAsync(id);
            int idDeleted = manufacturerDeleting.Id;
            _context.Manufacturers.Remove(manufacturerDeleting);
            await _context.SaveChangesAsync();
            return "Deleted product with ID: " + idDeleted;
        }
    }
}
