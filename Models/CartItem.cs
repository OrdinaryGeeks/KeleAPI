using System.ComponentModel.DataAnnotations;

namespace ShoppingAPI.Models
{
    public class CartItem : Item
    {
       public int ShoppingCartID { get; set; }


        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]

        public int UnitsInCart { get; set; }
    }
}
