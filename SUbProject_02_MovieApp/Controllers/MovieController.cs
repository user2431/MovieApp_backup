using DataAccessLayer;
using DataAccessLayer.DataModels;
using DataAccessLayer.Repository.MovieRepository;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using SUbProject_02_MovieApp.DTOModels.MovieDTOs;

namespace SUbProject_02_MovieApp.Controllers
{
    [ApiController]
    [Route("api/movie")]
    public class MovieController : BaseController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly LinkGenerator _linkGenerator;
        public MovieController(IMovieRepository movieRepository, LinkGenerator linkGenerator) : base(linkGenerator)
        {
            _movieRepository = movieRepository;
            _linkGenerator = linkGenerator;
        }
        [HttpGet("allmovies", Name = nameof(GetAllMovies))]
        public async Task<IActionResult> GetAllMovies(int page = 0, int pagesize = 10)
        {

            var movies = await _movieRepository.GetAllMovies(page, pagesize);

            var numberofmovies = await _movieRepository.CountAllMovies();

            var result = CreatePaging(nameof(GetAllMovies),
                page,
                pagesize,
                numberofmovies,
            movies,
            ' ');
            return Ok(result);
        }
        [HttpGet("genre/{Genre}", Name = nameof(GetAllMoviesByGenre))]
        public async Task<IActionResult> GetAllMoviesByGenre(string Genre, int page = 0, int pagesize = 10)
        {

            if (string.IsNullOrWhiteSpace(Genre))
            {
                return BadRequest("Genre is not provided.");
            }
            var movies = await _movieRepository.GetAllMoviesByGenre(Genre, page, pagesize);

            var numberofmovies = await _movieRepository.CountMoviesByGenre(Genre);
            if (movies == null || !movies.Any())
            {
                return NotFound($"No movies found for genre:{Genre}");
            }
            var result = CreatePaging(nameof(GetAllMoviesByGenre),
                page,
                pagesize,
                numberofmovies,
            movies,
                new { Genre });
            return Ok(result);
        }
        [HttpGet("genre", Name = nameof(GetAllGenre))]
        public async Task<IActionResult> GetAllGenre()
        {


            var genre = await _movieRepository.GetAllGenres();


            return Ok(genre);
        }
        [HttpGet("releaseyear/{ReleaseYear}", Name = "GetAllMoviesByReleaseYear")]
        public async Task<IActionResult> GetAllMoviesByReleaseYear(string ReleaseYear, int page = 0, int pagesize = 10)
        {

            if (string.IsNullOrWhiteSpace(ReleaseYear))
            {
                return BadRequest("ReleaseYear is not provided.");
            }
            var movies = await _movieRepository.GetAllMoviesByReleaseYear(ReleaseYear, page, pagesize);
            var numberofmovies = await _movieRepository.CountMoviesByReleaseYear(ReleaseYear);
            if (movies == null || !movies.Any())
            {
                return NotFound($"No movies found for release year:{ReleaseYear}");
            }
            var result = CreatePaging("GetAllMoviesByReleaseYear",
                page,
                pagesize,
                numberofmovies,
                movies,
                new { ReleaseYear });
            return Ok(result);
        }
        [HttpGet("search/{substring}", Name = nameof(SearchMoviesBySubstring))]
        public async Task<IActionResult> SearchMoviesBySubstring(string substring, int page = 0, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(substring))
            {
                return BadRequest("Search substring is required.");
            }

            var movies = await _movieRepository.SearchMoviesBySubString(substring, page, pageSize);

            var numberOfMovies = await _movieRepository.CountMoviesBySubString(substring);

            if (movies == null || !movies.Any())
            {
                return NotFound($"No movies found for substring: {substring}");
            }

            var movieDtos = movies.Select(movie => new MovieDTO
            {
                tconsturl = GetUrl("GetMovieById", args: new { id = movie.tconst }),
                tconst = movie.tconst,
                primarytitle = movie.primarytitle,
                startyear = movie.startyear

            }).ToList();

            var result = CreatePaging(nameof(SearchMoviesBySubstring), page, pageSize, numberOfMovies, movieDtos, new { substring });
            return Ok(result);
        }
        [HttpGet("{id}", Name = nameof(GetMovieById))]
        public async Task<IActionResult> GetMovieById(string id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }
        [HttpGet("actorname/{ActorName}", Name = nameof(SearchMoviesByActorName))]
        public async Task<IActionResult> SearchMoviesByActorName(string ActorName, int page = 0, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(ActorName))
            {
                return BadRequest("ActorName is required.");
            }

            var movies = await _movieRepository.SearchMoviesByActorName(ActorName, page, pageSize);

            var numberOfMovies = await _movieRepository.CountMoviesByActorName(ActorName);

            if (movies == null || !movies.Any())
            {
                return NotFound($"No movies found for actor: {ActorName}");
            }

            var movieDtos = movies.Select(movie => new MovieDTO
            {
                tconsturl = GetUrl("GetMovieById", args: new { id = movie.tconst }),
                tconst = movie.tconst,
                primarytitle = movie.primarytitle,
                startyear = movie.startyear

            }).ToList();

            var result = CreatePaging(nameof(SearchMoviesByActorName), page, pageSize, numberOfMovies, movieDtos, new { ActorName });
            return Ok(result);
        }
        [HttpGet("similar/{MovieId}", Name = nameof(SearchSimilarMovies))]
        public async Task<IActionResult> SearchSimilarMovies(string MovieId, int page = 0, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(MovieId))
            {
                return BadRequest("MovieId is required.");
            }

            var movies = await _movieRepository.SearchSimilarMovies(MovieId, page, pageSize);

            var numberOfMovies = await _movieRepository.CountSimilarMovies(MovieId);

            if (movies == null || !movies.Any())
            {
                return NotFound($"No similar movies found");
            }

            var movieDtos = movies.Select(movie => new MovieDTO
            {
                tconst = GetUrl("GetMovieById", args: new { id = movie.tconst }),
                primarytitle = movie.primarytitle,
                startyear = movie.startyear

            }).ToList();

            var result = CreatePaging(nameof(SearchMoviesByActorName), page, pageSize, numberOfMovies, movieDtos, new { MovieId });
            return Ok(result);
        }
    }
}
