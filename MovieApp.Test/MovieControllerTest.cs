using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SUbProject_02_MovieApp.Controllers;
using DataAccessLayer.Repository.MovieRepository;
using System.Threading.Tasks;
using DataAccessLayer.DataModels;

public class MovieControllerTests
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly MovieController _controller;

    public MovieControllerTests()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _controller = new MovieController(_movieRepositoryMock.Object, null);
    }
    [Fact]
    public async Task GetAllMoviesByGenre_ReturnsBadRequest_WhenGenreIsEmpty()
    {
        // Arrange
        string genre = ""; // Empty genre
        int page = 0;
        int pageSize = 10;

        // Act
        var result = await _controller.GetAllMoviesByGenre(genre, page, pageSize);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Genre is not provided.", badRequestResult.Value);
    }
    [Fact]
    public async Task SearchMoviesBySubstring_ReturnsNotFound_WhenNoMoviesFound()
    {
        // Arrange
        string substring = "nonexistent"; // A substring that won't match any movie
        int page = 0;
        int pageSize = 10;

        // Mock repository to return no movies
        _movieRepositoryMock.Setup(repo => repo.SearchMoviesBySubString(substring, page, pageSize))
                            .ReturnsAsync(new List<title_basics>()); 

        // Act
        var result = await _controller.SearchMoviesBySubstring(substring, page, pageSize);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"No movies found for substring: {substring}", notFoundResult.Value);
    }


}