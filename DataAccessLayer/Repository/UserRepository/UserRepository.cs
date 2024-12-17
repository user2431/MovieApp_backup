
using DataAccessLayer.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly MovieDBContext _dbContext;

        public UserRepository(MovieDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<user_info> getUserbyUsername(string username)
        {
            return await _dbContext.user_info.FirstOrDefaultAsync(u => u.username == username);
        }

        public async Task<user_info> getUserByIdentifier(string identifier)
        {
            return await _dbContext.user_info
                .FirstOrDefaultAsync(u => u.username == identifier || u.email == identifier);
        }

        public async Task<user_info> getUserByEmail(string email)
        {
            return await _dbContext.user_info.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<user_info> GetUserById(string userId) // New method
        {
            return await _dbContext.user_info.FirstOrDefaultAsync(u => u.user_id == userId);
        }

        public async Task AddUser(user_info user)
        {
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(user_info user)
        {
            _dbContext.user_info.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _dbContext.user_info.FindAsync(userId);
            if (user != null)
            {
                _dbContext.user_info.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

