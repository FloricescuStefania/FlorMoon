using FlorMoon.Models;

public interface IReviewService
{
    List<Review> GetAllReviews();
    List<Review> GetReviewsByProductId(int productId);
    Review GetReviewById(int id);
    void AddReview(Review review);
    void UpdateReview(Review review);
    void DeleteReview(int id);
}