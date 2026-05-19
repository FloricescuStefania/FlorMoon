using FlorMoon.Models;
using FlorMoon.Services.Interfaces;
using FlorMoon.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlorMoon.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IReviewService _reviewService;
        private readonly IWishlistService _wishlistService;

        public StoreController(IProductService productService,
                               ICategoryService categoryService,
                               IReviewService reviewService,
                               IWishlistService wishlistService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _reviewService = reviewService;
            _wishlistService = wishlistService;
        }

        public IActionResult Index(string category, string sort)
        {
            var products = _productService.GetAllProducts();

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Categories?.CategoryName == category).ToList();

            products = sort switch
            {
                "price_asc" => products.OrderBy(p => p.Price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.Price).ToList(),
                _ => products
            };

            int userId = 1;
            ViewBag.FavoriteIds = _wishlistService.GetUserWishlist(userId)
                                                 .Select(w => w.ProductID)
                                                 .ToList();

            ViewBag.Categories = _categoryService.GetAllCategories();
            return View(products);
        }

        public IActionResult Search(string searchTerm)
        {
            var products = _productService.GetAllProducts();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.ProductDescription.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            ViewBag.SearchTerm = searchTerm;
            return View(products);
        }

        [Route("Store/Product/{id}")]
        public IActionResult Product(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            ViewBag.Reviews = _reviewService.GetReviewsByProductId(id);
            return View(product);
        }

        // --- ADD REVIEW ---
        [HttpPost]
        [Authorize]
        public IActionResult AddReview(int ProductID, int Rating, string Text)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Administrators are not allowed to add reviews.";
                return RedirectToAction("Product", new { id = ProductID });
            }

            if (!string.IsNullOrEmpty(Text))
            {
                var review = new Review { ProductID = ProductID, Rating = Rating, Text = Text, UserID = 1 };
                _reviewService.AddReview(review);
                TempData["SuccessMessage"] = "Review added successfully!";
            }
            return RedirectToAction("Product", new { id = ProductID });
        }

        // --- EDIT REVIEW ---
        [HttpPost]
        [Authorize]
        public IActionResult EditReview(int ReviewID, int ProductID, int Rating, string Text)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Administrators cannot edit reviews.";
                return RedirectToAction("Product", new { id = ProductID });
            }

            var existingReview = _reviewService.GetReviewById(ReviewID);
            if (existingReview != null)
            {
                existingReview.Rating = Rating;
                existingReview.Text = Text;
                _reviewService.UpdateReview(existingReview);
                TempData["SuccessMessage"] = "The review has been modified.";
            }
            return RedirectToAction("Product", new { id = ProductID });
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteReview(int reviewId, int productId)
        {
            if (User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "To delete reviews as an Admin, use the Review Management panel.";
                return RedirectToAction("Product", new { id = productId });
            }

            _reviewService.DeleteReview(reviewId);
            TempData["SuccessMessage"] = "Your review has been deleted.";
            return RedirectToAction("Product", new { id = productId });
        }
    }
}