using FlorMoon.Models;

namespace FlorMoon.Repository.Interfaces
{
    public interface IShippingDetailRepository
    {
        ShippingDetail GetShippingDetail(int orderID);
    }
}
