using System.ComponentModel.DataAnnotations;

namespace FlorMoon.Models
{
    public class Review
    {
        [Key] public int ReviewID { get; set; }
        public int Rating { get; set; }
        public String Text { get; set; }
        public DateTime ReviewDate { get; set; }
        public int ProductID { get; set; }
        public Product? Products { get; set; }
        public int UserID { get; set; }
        public User? Users { get; set; }

    }
}
