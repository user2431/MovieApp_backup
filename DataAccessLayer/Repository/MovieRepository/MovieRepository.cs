using DataAccessLayer.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.MovieRepository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDBContext _dbContext;
        public MovieRepository(MovieDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountMoviesByGenre(string Genre)
        {
            return await _dbContext.title_basics.Where(m => m.genres.ToLower() == Genre.ToLower())
            .CountAsync();
        }
        public async Task<int> CountAllMovies()
        {
            return await _dbContext.title_basics
            .CountAsync();
        }
        public async Task<int> CountMoviesByReleaseYear(string ReleaseYear)
        {
            return await _dbContext.title_basics.Where(m => m.startyear == ReleaseYear)
            .CountAsync();
        }
        public async Task<int> CountMoviesBySubString(string Substring)
        {
            return await _dbContext.title_basics.CountAsync(m => m.primarytitle.ToLower().Contains(Substring) || m.originaltitle.ToLower().Contains(Substring));

        }
        public async Task<int> CountMoviesByActorName(string ActorName)
        {

            return await (
            from nb in _dbContext.nameBasics
            where nb.PrimaryName.Contains(ActorName)
            join tp in _dbContext.title_Principals on nb.nconst equals tp.nconst
            join tb in _dbContext.title_basics on tp.tconst equals tb.tconst
            select tb
             ).CountAsync();
        }

        public async Task<IEnumerable<title_basics>> GetAllMoviesByGenre(string Genre, int page, int pagesize)
        {
            return await _dbContext.title_basics
                .Where(m => m.genres.ToLower() == Genre.ToLower())
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToListAsync();
        }
        public async Task<IEnumerable<Genres>> GetAllGenres()
        {
            return await _dbContext.genres.ToListAsync();
        }
        public async Task<IEnumerable<title_basics>> GetAllMovies(int page, int pagesize)
        {
            return await _dbContext.title_basics
                .Skip(page * pagesize)
                .Take(pagesize)
                .Select(movie => new title_basics
                {
                    tconst = movie.tconst.Trim(),
                    primarytitle = movie.primarytitle,
                    startyear = movie.startyear,
                    genres = movie.genres,

                })
                .ToListAsync();
        }
        public async Task<IEnumerable<title_basics>> GetAllMoviesByReleaseYear(string ReleaseYear, int page, int pagesize)
        {
            return await _dbContext.title_basics
                .Where(m => m.startyear == ReleaseYear.ToString())
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToListAsync();
        }
        public async Task<IEnumerable<title_basics>> SearchMoviesBySubString(string SubString, int page, int pagesize)
        {
            return await _dbContext.title_basics
                .Where(m => m.originaltitle.ToLower().Contains(SubString) || m.primarytitle.ToLower().Contains(SubString))
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToListAsync();
        }
        public async Task<int> CountSimilarMovies(string MovieId)

        {
            var originalMovie = await _dbContext.title_basics.Where(m => m.tconst == MovieId).Include(m => m.genres).FirstOrDefaultAsync();

            return await _dbContext.title_basics
                        .Where(m => m.tconst != MovieId)
                         .Where(m => m.genres.Any(g => originalMovie.genres.Contains(g)))
                         .Where(m => m.startyear == originalMovie.startyear)
                .CountAsync();
        }
        public async Task<IEnumerable<title_basics>> SearchMoviesByActorName(string ActorName, int page, int pagesize)
        {
            return await (
                from nb in _dbContext.nameBasics
                where nb.PrimaryName.ToLower().Contains(ActorName.ToLower())
                join tp in _dbContext.title_Principals on nb.nconst equals tp.nconst
                join tb in _dbContext.title_basics on tp.tconst equals tb.tconst
                select tb

                ).Distinct().Skip(page * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<title_basics> GetMovieByIdAsync(string id)
        {

            return await _dbContext.title_basics
                .FirstOrDefaultAsync(m => m.tconst == id);
        }

        public async Task<IEnumerable<title_basics>> SearchSimilarMovies(string MovieId, int page, int pagesize)
        {
            var originalMovie = await _dbContext.title_basics.Where(m => m.tconst == MovieId).Include(m => m.genres).FirstOrDefaultAsync();
            if (originalMovie == null)
            {
                throw new Exception("Movie not found.");
            }
            var similarMoviesQuery = _dbContext.title_basics
                        .Where(m => m.tconst != MovieId)
                         .Where(m => m.genres.Any(g => originalMovie.genres.Contains(g)))
                         .Where(m => m.startyear == originalMovie.startyear);

            return await similarMoviesQuery.ToListAsync();
        }
    }
}