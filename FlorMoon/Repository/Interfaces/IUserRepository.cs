using FlorMoon.Models;

namespace FlorMoon.Repository.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        User GetUserByEmail(string email);
        User GetUserByID(int id);
        IEnumerable<User> GetAll();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
