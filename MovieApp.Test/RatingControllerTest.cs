using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SUbProject_02_MovieApp.Controllers;
using DataAccessLayer.Repository.RatingRepository;
using System.Threading.Tasks;

public class RatingControllerTests
{
    private readonly Mock<IRatingRepository> _ratingRepositoryMock;
    private readonly RatingController _controller;

    public RatingControllerTests()
    {
        _ratingRepositoryMock = new Mock<IRatingRepository>();
        _controller = new RatingController(_ratingRepositoryMock.Object, null);
    }

    [Fact]
    public async Task RateMovie_ReturnsBadRequest_WhenRatingIsOutOfRange()
    {
        // Arrange
        string userId = "user123";
        string movieId = "movie456";
        int invalidRating = 11;  

        // Act
        var result = await _controller.RateMovie(userId, movieId, invalidRating);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Rating must be between 1 and 10.", badRequestResult.Value);
    }
    [Fact]
    public async Task DeleteRating_ReturnsNotFound_WhenRatingDoesNotExist()
    {
        // Arrange
        string userId = "user123";
        string movieId = "movie456";

        
        _ratingRepositoryMock.Setup(repo => repo.DeleteRating(userId, movieId))
                             .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteRating(userId, movieId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Rating not found for the specified user and movie.", notFoundResult.Value);
    }

}
