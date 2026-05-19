using FlorMoon.Data;
using FlorMoon.Interfaces;
using FlorMoon.Repository.Interfaces;

namespace FlorMoon.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _context;
        private IProductRepository _product;
        private ICategoryRepository _category;
        private IOrderRepository _order;
        private IOrderItemRepository _orderItem;
        private IReviewRepository _review;
        private IShippingDetailRepository _shippingDetail;
        private IShoppingCartRepository _shoppingCart;
        private IShoppingCartItemRepository _shoppingCartItem;
        private IUserRepository _user;
        private IWishlistRepository _wishlistRepository;

        public RepositoryWrapper(AppDbContext context)
        {
            _context = context;
        }
        public IProductRepository ProductRepository
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_context);
                }
                return _product;
            }
        }
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_context);
                }
                return _category;
            }
        }
        public IOrderRepository OrderRepository
        {
            get
            {
                if (_order == null)
                {
                    _order = new OrderRepository(_context);
                }
                return _order;
            }
        }

        public IOrderItemRepository OrderItemRepository
        {
            get
            {
                if (_orderItem == null)
                {
                    _orderItem = new OrderItemRepository(_context);
                }
                return _orderItem;
            }
        }

        public IReviewRepository ReviewRepository
        {
            get
            {
                if (_review == null)
                {
                    _review = new ReviewRepository(_context);
                }
                return _review;
            }
        }

        public IShippingDetailRepository ShippingDetailRepository
        {
            get
            {
                if (_shippingDetail == null)
                {
                    _shippingDetail = new ShippingDetailRepository(_context);
                }
                return _shippingDetail;
            }
        }

        public IShoppingCartRepository ShoppingCartRepository
        {
            get
            {
                if (_shoppingCart == null)
                {
                    _shoppingCart = new ShoppingCartRepository(_context);
                }
                return _shoppingCart;
            }
        }

        public IShoppingCartItemRepository ShoppingCartItemRepository
        {
            get
            {
                if (_shoppingCartItem == null)
                {
                    _shoppingCartItem = new ShoppingCartItemRepository(_context);
                }
                return _shoppingCartItem;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return _user;
            }
        }

        public IWishlistRepository WishlistRepository
        {
            get
            {
                if (_wishlistRepository == null)
                {
                    _wishlistRepository = new WishlistRepository(_context);
                }
                return _wishlistRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
