using DataAccessLayer.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.RatingRepository
{
    public interface IRatingRepository
    {
        public Task<bool> CheckUserHasRatedMovie(string userId, string movieId);

        public Task AddorUpdateRating(string userId, string movieId, int rating);
        public Task<bool> DeleteRating(string userId, string movieId);
        Task<UserRating> GetRatingForUserandMovie(string UserId, string MovieId);
       
      



    }
}
