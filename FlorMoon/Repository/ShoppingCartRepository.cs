using FlorMoon.Data;
using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FlorMoon.Repository
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(AppDbContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public ShoppingCart GetShoppingCartByUserID(int userID)
        {
            return _context.ShoppingCart
                .Include(sc => sc.ShoppingCartItems)
                .FirstOrDefault(sc => sc.userID == userID);
        }
        public void CreateShoppingCart(ShoppingCart shoppingCart)
        {
            Create(shoppingCart);
        }

        public void AddToCart(int userID, int productID)
        {
            var shoppingCart = GetShoppingCartByUserID(userID);
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    userID = userID,
                    TotalPrice = 0
                };
                Create(shoppingCart);
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductID == productID);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var existingCartItem = _context.ShoppingCartItems
                .FirstOrDefault(sci => sci.shoppingCartID == shoppingCart.ShoppingCartID && sci.productID == productID);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                _context.ShoppingCartItems.Update(existingCartItem);
            }
            else
            {
                var newCartItem = new ShoppingCartItem
                {
                    shoppingCartID = shoppingCart.ShoppingCartID,
                    productID = productID,
                    Quantity = 1,
                    Price = product.Price
                };
                _context.ShoppingCartItems.Add(newCartItem);
            }

            shoppingCart.TotalPrice = GetTotalPrice(userID);
            Update(shoppingCart);
        }

        public void RemoveFromCart(int userID, int productID)
        {
            var shoppingCart = GetShoppingCartByUserID(userID);
            if (shoppingCart == null)
            {
                throw new Exception("Shopping cart not found");
            }

            var cartItem = _context.ShoppingCartItems
                .FirstOrDefault(sci => sci.shoppingCartID == shoppingCart.ShoppingCartID && sci.productID == productID);

            if (cartItem != null)
            {
                _context.ShoppingCartItems.Remove(cartItem);
                shoppingCart.TotalPrice = GetTotalPrice(userID);
                Update(shoppingCart);
            }
        }

        public void ClearCart(int userID)
        {
            var shoppingCart = GetShoppingCartByUserID(userID);
            if (shoppingCart != null)
            {
                var cartItems = _context.ShoppingCartItems
                    .Where(sci => sci.shoppingCartID == shoppingCart.ShoppingCartID)
                    .ToList();

                _context.ShoppingCartItems.RemoveRange(cartItems);
                shoppingCart.TotalPrice = 0;
                Update(shoppingCart);
            }
        }

        public float GetTotalPrice(int userID)
        {
            var shoppingCart = GetShoppingCartByUserID(userID);
            if (shoppingCart == null)
            {
                return 0;
            }
            var total = _context.ShoppingCartItems
                .Where(sci => sci.shoppingCartID == shoppingCart.ShoppingCartID)
                .Sum(sci => sci.Quantity * sci.Price);
            return total;
        }

        public IEnumerable<ShoppingCartItem> GetCartItemsByUserId(int userId)
        {
            return _context.ShoppingCartItems
                .Include(i => i.Products)
                .Include(i => i.ShoppingCart)
                .Where(i => i.ShoppingCart.Users.UserID == userId)
                .ToList();
        }
    }
}
