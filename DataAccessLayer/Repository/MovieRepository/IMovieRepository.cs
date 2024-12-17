using DataAccessLayer.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.MovieRepository
{
    public interface IMovieRepository
    {
        Task<IEnumerable<title_basics>> GetAllMoviesByGenre(string Genre,int page, int pagesize);
        Task<IEnumerable<Genres>> GetAllGenres();
        Task<IEnumerable<title_basics>> GetAllMovies(int page, int pagesize);
        Task<IEnumerable<title_basics>> GetAllMoviesByReleaseYear(string ReleaseYear, int page, int pagesize);
        Task<IEnumerable<title_basics>> SearchMoviesBySubString(string Substring, int page, int pagesize);
        Task<IEnumerable<title_basics>> SearchMoviesByActorName(string ActorName, int page, int pagesize);
        Task<IEnumerable<title_basics>> SearchSimilarMovies(string MovieId, int page, int pagesize);
        Task<int> CountMoviesByGenre(string Genre);
        Task<int> CountAllMovies();
        Task<int> CountMoviesByReleaseYear(string ReleaseYear);
        Task<int> CountMoviesBySubString(string Substring);
        Task<int> CountMoviesByActorName(string ActorName);
        Task<int> CountSimilarMovies(string MovieId);
        Task<title_basics> GetMovieByIdAsync(string id);
    }
}
