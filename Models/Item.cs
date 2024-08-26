using System.ComponentModel.DataAnnotations;

namespace ShoppingAPI.Models
{
    public class Item
    {
        public int ID { get; set; }
        public decimal Price { get; set; }


        public required string Description { get; set; }
        public required string Name { get; set; }

        public required string PhotoURL { get; set; }






    }
}
