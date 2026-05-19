using System.ComponentModel.DataAnnotations;

namespace FlorMoon.Models
{
    public class Order
    {
        [Key] public int OrderID { get; set; }
        public float OrderPrice { get; set; }
        public String Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserID { get; set; }
        public User? Users { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ShippingDetail? ShippingDetails { get; set; }
    }
}
