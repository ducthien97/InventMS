using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace InventMS.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float CustomerRating { get; set; }
        public float MSRPrice { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("Manufacturer")]
        public int ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
