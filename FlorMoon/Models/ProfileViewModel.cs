using System.ComponentModel.DataAnnotations;
using System;

namespace FlorMoon.Models
{
    public class ProfileViewModel
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        private string? _profilePicturePath;
        public string? ProfilePicturePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_profilePicturePath) || !_profilePicturePath.Contains("/Images/", StringComparison.OrdinalIgnoreCase))
                {
                    return "/Images/default-avatar.png";
                }

                return _profilePicturePath;
            }
            set
            {
                _profilePicturePath = value;
            }
        }

        [Required(ErrorMessage = "Current password is required")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Enter new password")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The passwords don't match!")]
        public string? ConfirmPassword { get; set; }
    }
}