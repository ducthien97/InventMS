using System.Collections.Generic;
namespace InventMS
{
    public interface IDataAccessProvider
    {
        public Models.Category GetCategoryById(int id);
        List<Models.Product> GetProductsData();
        public Models.Manufacturer GetManufacturerById(int id);
    }
}
