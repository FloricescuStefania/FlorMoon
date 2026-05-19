using System.ComponentModel.DataAnnotations;

namespace FlorMoon.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string CategoryName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}