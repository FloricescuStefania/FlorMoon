using FlorMoon.Models;

namespace FlorMoon.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User? GetCurrentUser();
        User? GetUserById(int id);
        User? GetUserByEmail(string email); 
        void CreateUser(User user);
        void UpdateUser(User user); 
        void DeleteUser(int id);
        bool EmailExists(string email); 
        User? Authenticate(string email, string password); 
    }
}