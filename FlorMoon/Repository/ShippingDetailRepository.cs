using FlorMoon.Data;
using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;

namespace FlorMoon.Repository
{
    public class ShippingDetailRepository : RepositoryBase<ShippingDetail>, IShippingDetailRepository
    {
        public ShippingDetailRepository(AppDbContext context) : base(context)
        {
        }

        public ShippingDetail GetShippingDetail(int orderID)
        {
            return FindByCondition(sd => sd.OrderID == orderID).FirstOrDefault();
        }
    }
}
