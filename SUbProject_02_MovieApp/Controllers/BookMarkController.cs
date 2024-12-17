using DataAccessLayer.Repository.BookMarkRepository;
using Microsoft.AspNetCore.Mvc;

namespace SUbProject_02_MovieApp.Controllers
{
    [ApiController]
    [Route("api/bookmark")]
    public class BookMarkController : Controller
    {
        private readonly IBookmarkRepository _bookmarkRepository;
        public BookMarkController(IBookmarkRepository bookmarkRepository)
        {
            _bookmarkRepository = bookmarkRepository;
        }
        [HttpPost]
        public async Task<IActionResult> AddBookmark(string userId, string movieId)
        {
            if (await _bookmarkRepository.BookmarkExistsAsync(userId, movieId))
            {
                return BadRequest("This movie is already bookmarked by the user.");
            }

            await _bookmarkRepository.AddBookmarkAsync(userId, movieId);
            return Ok("Movie bookmarked successfully.");
        }

        [HttpDelete("bookmark")]
        public async Task<IActionResult> RemoveBookmark(string userId, string movieId)
        {
            if (!await _bookmarkRepository.BookmarkExistsAsync(userId, movieId))
            {
                return NotFound("Bookmark does not exist.");
            }

            await _bookmarkRepository.DeleteBookmarkAsync(userId, movieId);
            return Ok("Bookmark removed successfully.");
        }

    }
}
