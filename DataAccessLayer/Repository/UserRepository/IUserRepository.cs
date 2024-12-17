using DataAccessLayer.DataModels;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<user_info> getUserbyUsername(string username);
        Task<user_info> getUserByIdentifier(string identifier);
        Task<user_info> getUserByEmail(string email);
        Task<user_info> GetUserById(string userId);
        Task AddUser(user_info user);
        Task UpdateUser(user_info user);
        Task DeleteUser(string userId);
    }
}
