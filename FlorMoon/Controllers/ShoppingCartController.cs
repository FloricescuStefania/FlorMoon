using FlorMoon.Data;
using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using FlorMoon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlorMoon.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService, IRepositoryWrapper repositoryWrapper, IUserService userService, IOrderService orderService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
            _userService = userService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Administrator"))
            {
                TempData["ErrorMessage"] = "Administrator accounts cannot add products to the cart.";
                return RedirectToAction("Index", "Home");
            }

            var user = _userService.GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _shoppingCartService.GetShoppingCartByUserID(user.UserID);
            if (cart == null)
                return View(new List<ShoppingCartItem>());

            var items = cart.ShoppingCartItems?.ToList() ?? new List<ShoppingCartItem>();
            return View(items);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            if (User.IsInRole("Administrator"))
            {
                TempData["ErrorMessage"] = "Administrator accounts cannot add products to the cart.";
                return Redirect(Request.Headers["Referer"].ToString() ?? "/Store");
            }

            var user = _userService.GetCurrentUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = _shoppingCartService.GetShoppingCartByUserID(user.UserID);
            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    userID = user.UserID,
                };
                _shoppingCartService.CreateShoppingCart(cart);
            }

            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _shoppingCartService.AddToCart(product, quantity, user.UserID);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int itemId)
        {
            var user = _userService.GetCurrentUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = _shoppingCartService.GetShoppingCartByUserID(user.UserID);
            var item = cart?.ShoppingCartItems?.FirstOrDefault(i => i.ShoppingCartItemID == itemId);
            if (item != null)
            {
                _shoppingCartService.RemoveItemFromCart(item);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            var user = _userService.GetCurrentUser();
            if (user == null) return RedirectToAction("Login", "Account");

            _shoppingCartService.ClearShoppingCart(user.UserID);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int itemId, string action)
        {
            var user = _userService.GetCurrentUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = _shoppingCartService.GetShoppingCartByUserID(user.UserID);
            var item = cart?.ShoppingCartItems?.FirstOrDefault(i => i.ShoppingCartItemID == itemId);

            if (item != null)
            {
                if (action == "increase")
                    item.Quantity += 1;
                else if (action == "decrease" && item.Quantity > 1)
                    item.Quantity -= 1;

                _shoppingCartService.Update(item);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            if (User.IsInRole("Administrator"))
            {
                TempData["ErrorMessage"] = "Administrative accounts do not place orders.";
                return RedirectToAction("Index", "Home");
            }

            var user = _userService.GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _shoppingCartService.GetShoppingCartByUserID(user.UserID);
            var cartItems = cart?.ShoppingCartItems?.ToList() ?? new List<ShoppingCartItem>();

            if (!cartItems.Any())
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            string cleanAddress = user.Address ?? "";
            if (cleanAddress.Contains("user profile") || cleanAddress.Contains("implicit missing"))
            {
                cleanAddress = "";
            }

            var shippingDetail = new ShippingDetail
            {
                Address = cleanAddress
            };

            ViewBag.User = user;
            ViewBag.CartItems = cartItems;
            return View(shippingDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(ShippingDetail shippingDetail)
        {
            ModelState.Remove("ShippingDetails");
            ModelState.Remove("OrderItems");
            ModelState.Remove("Products");

            if (User.IsInRole("Administrator"))
            {
                TempData["ErrorMessage"] = "Administrative accounts do not place orders.";
                return RedirectToAction("Index", "Home");
            }

            var user = _userService.GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _shoppingCartService.GetShoppingCartByUserID(user.UserID);
            var cartItems = cart?.ShoppingCartItems?.ToList() ?? new List<ShoppingCartItem>();

            if (!cartItems.Any())
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(shippingDetail.Address) ||
                shippingDetail.Address.Trim().Equals("Delivery to the address in the user's profile", StringComparison.OrdinalIgnoreCase) ||
                shippingDetail.Address.Length < 5)
            {
                ModelState.AddModelError("Address", "Please enter a valid and specific delivery address.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.User = user;
                ViewBag.CartItems = cartItems;
                return View(shippingDetail);
            }

            float calculatedTotal = 0;
            var curatedOrderItems = new List<OrderItem>();

            foreach (var item in cartItems)
            {
                float productPrice = item.Price;

                if (item.Products != null)
                {
                    productPrice = item.Products.Price;
                }
                else
                {
                    var p = _productService.GetProductById(item.productID);
                    if (p != null) productPrice = p.Price;
                }

                calculatedTotal += productPrice * item.Quantity;

                curatedOrderItems.Add(new OrderItem
                {
                    ProductID = item.productID,
                    Quantity = item.Quantity,
                    Price = productPrice
                });
            }

            var order = new Order
            {
                UserID = user.UserID,
                Status = "Pending",
                OrderPrice = calculatedTotal,
                OrderItems = curatedOrderItems,
                OrderDate = DateTime.Now
            };

            if (shippingDetail != null)
            {
                order.ShippingDetails = new ShippingDetail
                {
                    Address = shippingDetail.Address.Trim()
                };
            }

            try
            {
                _orderService.CompleteOrder(order, user.UserID);

                if (string.IsNullOrEmpty(user.Address) || user.Address.Contains("user profile") || user.Address.Contains("implicit missing"))
                {
                    user.Address = shippingDetail.Address.Trim();
                    _userService.UpdateUser(user);
                }

                return RedirectToAction("OrderConfirmation", new { id = order.OrderID });
            }
            catch (Exception ex)
            {
                string fullMessage = ex.Message + (ex.InnerException != null ? " -> " + ex.InnerException.Message : "");
                ModelState.AddModelError("", "Error saving: " + fullMessage);

                foreach (var item in cartItems)
                {
                    if (item.Products == null) item.Products = _productService.GetProductById(item.productID);
                }
                ViewBag.User = user;
                ViewBag.CartItems = cartItems;
                return View(shippingDetail);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder(int id)
        {
            var order = _orderService.GetOrderById(id);

            if (order != null)
            {
                order.Status = "Canceled";
                _orderService.UpdateOrder(order);

                return RedirectToAction("OrderConfirmation", new { id = id });
            }

            return NotFound();
        }

        public IActionResult OrderConfirmation(int id)
        {
            var order = _orderService.GetOrderById(id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "The requested command could not be found.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.OrderId = id;
            return View(order);
        }
    }
}