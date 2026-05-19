using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using FlorMoon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlorMoon.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ReviewService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public List<Review> GetAllReviews()
        {
            return _repositoryWrapper.ReviewRepository
                .FindAll()
                .ToList();
        }

        //review for a specific product
        public List<Review> GetReviewsByProductId(int productId)
        {
            return _repositoryWrapper.ReviewRepository
                .FindByCondition(r => r.ProductID == productId)
                .ToList();
        }

        public Review GetReviewById(int id)
        {
            return _repositoryWrapper.ReviewRepository
                .FindByCondition(r => r.ReviewID == id)
                .FirstOrDefault();
        }

        public void AddReview(Review review)
        {
            _repositoryWrapper.ReviewRepository.Create(review);
            _repositoryWrapper.Save();
        }

        public void UpdateReview(Review review)
        {
            _repositoryWrapper.ReviewRepository.Update(review);
            _repositoryWrapper.Save();
        }

        public void DeleteReview(int id)
        {
            var review = GetReviewById(id);
            if (review != null)
            {
                _repositoryWrapper.ReviewRepository.Delete(review);
                _repositoryWrapper.Save();
            }
        }
    }
}