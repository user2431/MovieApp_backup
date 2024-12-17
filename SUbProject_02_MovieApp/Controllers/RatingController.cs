using DataAccessLayer.DataModels;
using DataAccessLayer.Repository.NewFolder;
using DataAccessLayer.Repository.RatingRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SUbProject_02_MovieApp.Controllers
{
    [ApiController]
    [Route("api/rating")]
    public class RatingController : BaseController
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly LinkGenerator _linkGenerator;
        public RatingController(IRatingRepository ratingRepository, LinkGenerator linkGenerator) : base(linkGenerator)
        {
            _ratingRepository = ratingRepository;
            _linkGenerator = linkGenerator;
        }
        [HttpPost("rate")]
        public async Task<IActionResult> RateMovie(string userId, string movieId, int rating)
        {
            if (rating < 1 || rating > 10) 
            {
                return BadRequest("Rating must be between 1 and 10.");
            }

            
            bool hasRated = await _ratingRepository.CheckUserHasRatedMovie(userId, movieId);

            if (hasRated)
            {
                return BadRequest("User has already rated this movie.");
            }

            await _ratingRepository.AddorUpdateRating(userId, movieId, rating);

            return Ok("Rating submitted successfully.");
        }
        [HttpDelete("rate")]
        public async Task<IActionResult> DeleteRating(string userId, string movieId)
        {
            
            var isDeleted = await _ratingRepository.DeleteRating(userId, movieId);

            if (!isDeleted)
            {
                return NotFound("Rating not found for the specified user and movie.");
            }

            return Ok("Rating deleted successfully.");
        }
        [HttpGet("{UserId}/{MovieId}")]
        public async Task<IActionResult> GetRatingForUserandMovie(string UserId, string MovieId)
        {

            if (string.IsNullOrWhiteSpace(MovieId))
            {
                return BadRequest("Movie Id is not provided.");
            }
            if (string.IsNullOrWhiteSpace(UserId))
            {
                return BadRequest("User Id is not provided.");
            }
            var rating = await _ratingRepository.GetRatingForUserandMovie(UserId, MovieId);

            if (rating == null)
            {
                return NotFound("No movies found");
            }

            return Ok(rating);
        }



    }
}
