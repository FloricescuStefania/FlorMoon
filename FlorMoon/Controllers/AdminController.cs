using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FlorMoon.Models;
using FlorMoon.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlorMoon.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<IdentityUser> userManager,
            IOrderService orderService,
            IReviewService reviewService,
            IUserService userService,
            IProductService productService,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _orderService = orderService;
            _reviewService = reviewService;
            _userService = userService;
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            try
            {
                var identityUsers = await _userManager.Users.ToListAsync();
                var userViewModels = new List<UserViewModel>();

                foreach (var identityUser in identityUsers)
                {
                    var roles = await _userManager.GetRolesAsync(identityUser);
                    var customUserProfile = _userService.GetUserByEmail(identityUser.Email ?? "");

                    userViewModels.Add(new UserViewModel
                    {
                        Id = identityUser.Id,
                        Email = identityUser.Email ?? "No Email",
                        PhoneNumber = customUserProfile?.PhoneNumber ?? identityUser.PhoneNumber ?? "No Phone Number",
                        Name = customUserProfile != null && (!string.IsNullOrEmpty(customUserProfile.FirstName) || !string.IsNullOrEmpty(customUserProfile.LastName))
                                ? $"{customUserProfile.FirstName} {customUserProfile.LastName}".Trim()
                                : identityUser.UserName ?? "FlorMoon User",
                        Address = customUserProfile?.Address ?? "Not specified",
                        ProfilePicturePath = customUserProfile?.ProfilePicturePath ?? "default-avatar.png",
                        Roles = roles.ToList()
                    });
                }

                return View(userViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users for FlorMoon");
                TempData["ErrorMessage"] = "An error occurred while retrieving users.";
                return View(new List<UserViewModel>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var identityUser = await _userManager.FindByIdAsync(userId);
                if (identityUser == null)
                {
                    TempData["ErrorMessage"] = "User was not found.";
                    return RedirectToAction(nameof(Users));
                }

                if (identityUser.UserName == User.Identity?.Name)
                {
                    TempData["ErrorMessage"] = "You cannot delete your own administrator account!";
                    return RedirectToAction(nameof(Users));
                }

                var result = await _userManager.DeleteAsync(identityUser);

                if (result.Succeeded)
                {
                    var customUser = _userService.GetUserByEmail(identityUser.Email ?? "");
                    TempData["SuccessMessage"] = $"User with email {identityUser.Email} has been permanently deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred from the Identity system while deleting the user.";
                }

                return RedirectToAction(nameof(Users));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user deletion");
                TempData["ErrorMessage"] = "A critical error occurred while deleting the user.";
                return RedirectToAction(nameof(Users));
            }
        }

        [HttpGet]
        public IActionResult AllOrders()
        {
            try
            {
                var orders = _orderService.GetAllOrders();
                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching orders for FlorMoon");
                TempData["ErrorMessage"] = "An error occurred while retrieving orders.";
                return View(new List<Order>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                var order = _orderService.GetOrderById(orderId);
                if (order == null)
                {
                    return NotFound();
                }

                order.Status = status;
                _orderService.UpdateOrder(order);

                TempData["SuccessMessage"] = $"Order status #{orderId} was updated to '{status}'.";
                return RedirectToAction(nameof(AllOrders));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when changing order status in Admin");
                TempData["ErrorMessage"] = "The order status could not be changed.";
                return RedirectToAction(nameof(AllOrders));
            }
        }

        [HttpGet]
        public IActionResult AllReviews()
        {
            try
            {
                var reviews = _reviewService.GetAllReviews();
                var reviewsWithDetails = new List<ReviewAdminViewModel>();

                foreach (var review in reviews)
                {
                    string clientName = "Client ";
                    string clientEmail = "No Email";

                    var userProp = review.GetType().GetProperties()
                        .FirstOrDefault(p => p.Name.Contains("User", StringComparison.OrdinalIgnoreCase) ||
                                             p.Name.Contains("Email", StringComparison.OrdinalIgnoreCase) ||
                                             p.Name.Contains("Client", StringComparison.OrdinalIgnoreCase));

                    if (userProp != null)
                    {
                        var val = userProp.GetValue(review)?.ToString();
                        if (!string.IsNullOrEmpty(val))
                        {
                            var u = int.TryParse(val, out int id) ? _userService.GetUserById(id) : _userService.GetUserByEmail(val);
                            if (u != null)
                            {
                                clientName = $"{u.FirstName} {u.LastName}".Trim();
                                clientEmail = u.Email;
                                if (string.IsNullOrEmpty(clientName)) clientName = u.Email;
                            }
                            else
                            {
                                clientEmail = val;
                            }
                        }
                    }

                    string productName = $"Product #{review.ProductID}";
                    try
                    {
                        var product = _productService.GetProductById(review.ProductID);
                        if (product != null)
                        {
                            productName = product.ProductName;
                        }
                    }
                    catch
                    {
                        // Fallback context
                    }

                    reviewsWithDetails.Add(new ReviewAdminViewModel
                    {
                        ReviewID = review.ReviewID,
                        ClientName = clientName,
                        ClientEmail = clientEmail,
                        ProductName = productName,
                        Rating = review.Rating,
                        Text = review.Text
                    });
                }

                return View(reviewsWithDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reviews for FlorMoon Admin");
                TempData["ErrorMessage"] = "An error occurred while retrieving reviews.";
                return View(new List<ReviewAdminViewModel>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReview(int reviewId)
        {
            try
            {
                _reviewService.DeleteReview(reviewId);
                TempData["SuccessMessage"] = "The review was successfully deleted.";
                return RedirectToAction(nameof(AllReviews));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting review");
                TempData["ErrorMessage"] = "Could not delete review.";
                return RedirectToAction(nameof(AllReviews));
            }
        }
    }








    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ProfilePicturePath { get; set; } = "default-avatar.png";
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class ReviewAdminViewModel
    {
        public int ReviewID { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Text { get; set; }
    }
}