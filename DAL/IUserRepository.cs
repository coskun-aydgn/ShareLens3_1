using ShareLens3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareLens3.DAL
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User?> GetUserById(int userId);
        Task<User?> GetUserByEmail(string email);
        Task<bool> AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
        Task<bool> SaveChanges();
    }
}
