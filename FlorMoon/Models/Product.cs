using System.ComponentModel.DataAnnotations;

namespace FlorMoon.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Image path is required.")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryID { get; set; }
        public Category? Categories { get; set; }

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}