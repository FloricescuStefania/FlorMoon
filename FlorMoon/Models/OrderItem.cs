using System.ComponentModel.DataAnnotations;

namespace FlorMoon.Models
{
    public class OrderItem
    {
        [Key] public int OrderItemsID { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public int OrderID { get; set; }
        public Order? Orders { get; set; }
        public int ProductID { get; set; }
        public Product? Products { get; set; }
    }
}
