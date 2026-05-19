using FlorMoon.Models;
using FlorMoon.Services.Interfaces;
using FlorMoon.Repository.Interfaces;
using Microsoft.AspNetCore.Http; 
using System.Linq;

namespace FlorMoon.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IRepositoryWrapper repoWrapper, IHttpContextAccessor httpContextAccessor)
        {
            _repoWrapper = repoWrapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public User? GetCurrentUser()
        {
            var loggedInEmail = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(loggedInEmail))
            {
                return null;
            }

            return _repoWrapper.UserRepository
                .FindByCondition(u => u.Email == loggedInEmail)
                .FirstOrDefault();
        }

        public void UpdateUser(User user)
        {
            _repoWrapper.UserRepository.Update(user);
            _repoWrapper.Save();
        }

        public User? GetUserByEmail(string email) =>
            _repoWrapper.UserRepository.FindByCondition(u => u.Email == email).FirstOrDefault();

        public User GetUserById(int id) =>
            _repoWrapper.UserRepository.FindByCondition(u => u.UserID == id).FirstOrDefault()!;

        public IEnumerable<User> GetAllUsers() => _repoWrapper.UserRepository.FindAll();
        public void CreateUser(User user) { _repoWrapper.UserRepository.Create(user); _repoWrapper.Save(); }
        public void DeleteUser(int id) { }
        public bool EmailExists(string email) => GetUserByEmail(email) != null;
        public User? Authenticate(string email, string password) => null;
    }
}