using FlorMoon.Models;
using FlorMoon.Repository.Interfaces;
using FlorMoon.Services.Interfaces;

namespace FlorMoon.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CategoryService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var categoryRepository = _repositoryWrapper.CategoryRepository as ICategoryRepository;
            if (categoryRepository == null)
            {
                throw new InvalidCastException("CategoryRepository is not of type ICategoryRepository.");
            }

            return categoryRepository.FindAll().ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            var categoryRepository = _repositoryWrapper.CategoryRepository as ICategoryRepository;
            if (categoryRepository == null)
            {
                throw new InvalidCastException("CategoryRepository is not of type ICategoryRepository.");
            }
            return categoryRepository
                .FindByCondition(c => c.CategoryID == categoryId)
                .FirstOrDefault();
        }

        public void CreateCategory(Category category)
        {
            var categoryRepository = _repositoryWrapper.CategoryRepository as ICategoryRepository;
            if (categoryRepository == null)
            {
                throw new InvalidCastException("CategoryRepository is not of type ICategoryRepository.");
            }
            categoryRepository.Create(category);
            _repositoryWrapper.Save();
        }

        public void UpdateCategory(Category category)
        {
            var categoryRepository = _repositoryWrapper.CategoryRepository as ICategoryRepository;
            if (categoryRepository == null)
            {
                throw new InvalidCastException("CategoryRepository is not of type ICategoryRepository.");
            }
            categoryRepository.Update(category);
            _repositoryWrapper.Save();
        }

        public void DeleteCategory(int categoryId)
        {
            var categoryRepository = _repositoryWrapper.CategoryRepository as ICategoryRepository;
            if (categoryRepository == null)
            {
                throw new InvalidCastException("CategoryRepository is not of type ICategoryRepository.");
            }
            var category = categoryRepository.FindByCondition(c => c.CategoryID == categoryId).FirstOrDefault();
            if (category != null)
            {
                categoryRepository.Delete(category);
                _repositoryWrapper.Save();
            }
        }
    }
}
