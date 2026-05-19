using System.ComponentModel.DataAnnotations;

namespace FlorMoon.Models
{
    public class ShoppingCartItem
    {
        [Key] public int ShoppingCartItemID { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

        public int shoppingCartID { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }

        public int productID { get; set; }
        public Product? Products { get; set; }
    }
}
