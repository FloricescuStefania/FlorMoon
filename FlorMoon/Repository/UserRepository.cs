using FlorMoon.Data;
using FlorMoon.Models;
using Microsoft.EntityFrameworkCore;
using FlorMoon.Repository.Interfaces;

namespace FlorMoon.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public User GetUserByEmail(string email)
        {
            return FindByCondition(u => u.Email == email).FirstOrDefault();
        }
        public void AddUser(User user)
        {
            Create(user);
        }
        public void UpdateUser(User user)
        {
            Update(user);
        }
        public void DeleteUser(User user)
        {
            Delete(user);
        }

        public User GetUserByID(int id)
        {
            return FindByCondition(u => u.UserID == id).FirstOrDefault();
        }

        public IEnumerable<User> GetAll()
        {
            return FindAll().ToList();
        }
    }
}
