using System.ComponentModel.DataAnnotations;

namespace FlorMoon.Models
{
    public class ShippingDetail
    {
        [Key] public int ShippingID { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public String Address { get; set; }

        public int OrderID { get; set; }
        public Order? Orders { get; set; }
    }
}
