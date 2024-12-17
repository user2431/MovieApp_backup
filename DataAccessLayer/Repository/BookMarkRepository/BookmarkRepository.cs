using DataAccessLayer.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.BookMarkRepository
{
    public class BookmarkRepository : IBookmarkRepository
    {
        private MovieDBContext _dbContext;
        public BookmarkRepository(MovieDBContext movieDBContext)
        {
            _dbContext = movieDBContext;
        }
        public async Task AddBookmarkAsync(string userId, string movieId)
        {
            var bookmark = new BookMark
            {
                userid = userId,
                title_id = movieId
            };

            _dbContext.bookMarks.Add(bookmark);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> BookmarkExistsAsync(string userId, string movieId)
        {
            return await _dbContext.bookMarks
                .AnyAsync(ub => ub.userid == userId && ub.title_id == movieId);
        }

        public async Task DeleteBookmarkAsync(string userId, string movieId)
        {
            var bookmark = await _dbContext.bookMarks
                .FirstOrDefaultAsync(ub => ub.userid == userId && ub.title_id == movieId);

            if (bookmark != null)
            {
                _dbContext.bookMarks.Remove(bookmark);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
