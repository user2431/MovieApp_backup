using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.BookMarkRepository
{
    public interface IBookmarkRepository 
    {
        Task AddBookmarkAsync(string userId, string movieId);
        Task<bool> BookmarkExistsAsync(string userId, string movieId);
        Task DeleteBookmarkAsync(string userId, string movieId);
    }
}
