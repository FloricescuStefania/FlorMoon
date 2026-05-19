using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace FlorMoon.Models
{
    public class User
    {
        [Key] public int UserID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public String Password { get; set; }
        public String PhoneNumber { get; set; }
        public String Address { get; set; }
        public string? ProfilePicturePath { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
