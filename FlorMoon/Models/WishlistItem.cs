using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlorMoon.Models
{
    public class WishlistItem
    {
        [Key]
        public int WishlistItemID { get; set; }

        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product? Product { get; set; }

        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User? User { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}