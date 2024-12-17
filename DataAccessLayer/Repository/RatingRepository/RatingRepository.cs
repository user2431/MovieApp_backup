using DataAccessLayer.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.RatingRepository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly MovieDBContext _movieDBContext;
        public RatingRepository(MovieDBContext dBContext)
        {
            _movieDBContext = dBContext;
        }
        public async Task<bool> CheckUserHasRatedMovie(string userId, string movieId)
        {
            return await _movieDBContext.UserRatings
                .AnyAsync(ur => ur.user_id == userId && ur.tconst == movieId);
        }

        public async Task AddorUpdateRating(string userId, string movieId, int rating)
        {

            var existingRating = await _movieDBContext.UserRatings.FirstOrDefaultAsync(ur => ur.user_id == userId && ur.tconst == movieId);

            if (existingRating != null)
            {
                
                existingRating.rating = rating;
                _movieDBContext.UserRatings.Update(existingRating);
            }
            else
            {
               
                var newRating = new UserRating
                {
                    user_id = userId,
                    tconst = movieId,
                    rating = rating,
                    rated_date =DateTime.UtcNow
                };
                await _movieDBContext.UserRatings.AddAsync(newRating);
            }

            await _movieDBContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteRating(string userId, string movieId)
        {
            
            var rating = await _movieDBContext.UserRatings
                .FirstOrDefaultAsync(ur => ur.user_id == userId && ur.tconst == movieId);

            if (rating == null)
            {
                
                return false;
            }


            _movieDBContext.UserRatings.Remove(rating);
            await _movieDBContext.SaveChangesAsync();
            return true;
        }
        public async Task<UserRating> GetRatingForUserandMovie(string UserId, string MovieId)
        {
            return await _movieDBContext.UserRatings.SingleOrDefaultAsync(m => m.user_id == UserId && m.tconst == MovieId);

        }


    }
}
