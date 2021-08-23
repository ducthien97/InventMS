using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace InventMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private static AppDataContext AppDataContext = new AppDataContext();
        public static DataAccessProvider DataAccessProvider = new DataAccessProvider(AppDataContext);

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Models.Product> GetProducts()
        { 
            var products = DataAccessProvider.GetProductsData().ToArray();
            foreach (var product in products)
            {
                product.Category = DataAccessProvider.GetCategoryById(product.CategoryId);
                product.Manufacturer = DataAccessProvider.GetManufacturerById(product.ManufacturerId);
            }
            return products;
        }

        [HttpGet("{id}")]
        public Models.Product GetProduct(int id)
        {
            var product = DataAccessProvider.GetProductById(id);
            product.Manufacturer = DataAccessProvider.GetManufacturerById(product.ManufacturerId);
            product.Category = DataAccessProvider.GetCategoryById(product.CategoryId);
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Models.Product>> CreateProduct(Models.Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest();
                }
                var createdProduct = await DataAccessProvider.AddProductTask(product);
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new product");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Product>> UpdateProduct(int id, Models.Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest("ID mismatched");
                }
                var productToUpdate = DataAccessProvider.GetProductTask(id);
                if (productToUpdate == null)
                {
                    return NotFound($"Product with ID = {id} not found");
                }
                return await DataAccessProvider.UpdateProductTask(product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating data");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteProduct(int id)
        {
            try
            {
                var ProductToDelete = await DataAccessProvider.GetProductTask(id);
                if (ProductToDelete == null)
                {
                    return NotFound($"Product with Id = {id} not found");
                }

                return await DataAccessProvider.DeleteProductTask(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
            }
        }
    }
}
