using System.ComponentModel.DataAnnotations;

namespace ShoppingAPI.Models
{
    public class InventoryItem : Item
    {
        public int StoreFrontID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]

        public int UnitsAvailable { get; set; }

    }
}
