using FlorMoon.Models;
using FlorMoon.Services;
using FlorMoon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FlorMoon.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;
        private readonly IUserService _userService;

        public WishlistController(IWishlistService wishlistService, IUserService userService)
        {
            _wishlistService = wishlistService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var loggedInEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(loggedInEmail)) return Challenge();

            var user = _userService.GetUserByEmail(loggedInEmail);

            if (user == null)
            {
                user = new User
                {
                    Email = loggedInEmail,
                    FirstName = "",
                    LastName = "",
                    PhoneNumber = "",
                    Address = "",
                    Password = "IdentityManaged",
                    ProfilePicturePath = "/Images/default-avatar.png"
                };
                _userService.CreateUser(user);
                user = _userService.GetUserByEmail(loggedInEmail);
            }

            var wishlistItems = _wishlistService.GetUserWishlist(user.UserID);
            var products = wishlistItems.Select(w => w.Product).ToList();

            return View(products);
        }

        [HttpPost]
        public IActionResult ToggleWishlist(int productId)
        {
            var loggedInEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(loggedInEmail)) return Challenge();

            var user = _userService.GetUserByEmail(loggedInEmail);

            if (user == null)
            {
                user = new User
                {
                    Email = loggedInEmail,
                    FirstName = "",
                    LastName = "",
                    PhoneNumber = "",
                    Address = "",
                    Password = "IdentityManaged",
                    ProfilePicturePath = "/Images/default-avatar.png"
                };
                _userService.CreateUser(user);
                user = _userService.GetUserByEmail(loggedInEmail);
            }

            int userId = user.UserID;

            if (_wishlistService.IsInWishlist(userId, productId))
            {
                _wishlistService.RemoveFromWishlist(userId, productId);
            }
            else
            {
                _wishlistService.AddToWishlist(userId, productId);
            }

            var referer = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer))
            {
                return RedirectToAction("Index");
            }
            return Redirect(referer);
        }
    }
}