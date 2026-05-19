using FlorMoon.Models;

namespace FlorMoon.Repository.Interfaces
{

    public interface IReviewRepository : IRepositoryBase<Review>
    {
        IEnumerable<Review> GetReviewsByProductId(int productId);
        void CreateReview(Review review);
        void DeleteReview(Review review);
    }
}
