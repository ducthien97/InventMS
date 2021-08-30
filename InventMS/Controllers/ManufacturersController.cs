using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManufacturersController : ControllerBase
    {
        private readonly ILogger<ManufacturersController> _logger;
        private static AppDataContext AppDataContext = new AppDataContext();
        public static DataAccessProvider DataAccessProvider = new DataAccessProvider(AppDataContext);

        public ManufacturersController(ILogger<ManufacturersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetManufacturers()
        {
            try
            {
                return Ok(await DataAccessProvider.GetManufacturersTask());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.Manufacturer>> GetManufacturer(int id)
        {
            try
            {
                var result = await DataAccessProvider.GetManufacturerTask(id);

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Models.Manufacturer>> CreateManufacturer(Models.Manufacturer manufacturer)
        {
            try
            {
                if (manufacturer == null)
                {
                    return BadRequest();
                }
                var createdManufacturer = await DataAccessProvider.AddManufacturerTask(manufacturer);
                return CreatedAtAction("GetManufacturer", new { id = manufacturer.Id }, manufacturer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new manufacturer");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Manufacturer>> UpdateManufacturer(int id, Models.Manufacturer manufacturer)
        {
            try
            {
                if (id != manufacturer.Id)
                {
                    return BadRequest("ID mismatched");
                }
                var manufacturerToUpdate = DataAccessProvider.GetProductTask(id);
                if (manufacturerToUpdate == null)
                {
                    return NotFound($"Product with ID = {id} not found");
                }
                return await DataAccessProvider.UpdateManufacturerTask(manufacturer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating data");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteManufacturer(int id)
        {
            try
            {
                var ManufacturerToDelete = await DataAccessProvider.DeleteManufacturerTask(id);
                if (ManufacturerToDelete == null)
                {
                    return NotFound($"Product with Id = {id} not found");
                }

                return await DataAccessProvider.DeleteManufacturerTask(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
            }
        }


    }
} 
