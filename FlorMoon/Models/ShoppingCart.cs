using System.ComponentModel.DataAnnotations;

namespace FlorMoon.Models
{
    public class ShoppingCart
    {
        [Key] public int ShoppingCartID { get; set; }
        public float TotalPrice { get; set; }

        public int userID { get; set; }
        public User? Users { get; set; }

        public ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
    }
}
