using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SUbProject_02_MovieApp.Controllers;
using DataAccessLayer.Repository.BookMarkRepository;
using System.Threading.Tasks;

public class BookmarkControllerTests
{
    private readonly Mock<IBookmarkRepository> _bookmarkRepositoryMock;
    private readonly BookMarkController _controller;

    public BookmarkControllerTests()
    {
        _bookmarkRepositoryMock = new Mock<IBookmarkRepository>();
        _controller = new BookMarkController(_bookmarkRepositoryMock.Object);
    }
    [Fact]
    public async Task AddBookmark_ReturnsBadRequest_WhenMovieAlreadyBookmarked()
    {
        // Arrange
        var userId = "user1";
        var movieId = "movie1";

      
        _bookmarkRepositoryMock.Setup(repo => repo.BookmarkExistsAsync(userId, movieId))
                               .ReturnsAsync(true); 

        // Act
        var result = await _controller.AddBookmark(userId, movieId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("This movie is already bookmarked by the user.", badRequestResult.Value);
    }
    [Fact]
    public async Task RemoveBookmark_ReturnsNotFound_WhenBookmarkDoesNotExist()
    {
        // Arrange
        var userId = "user1";
        var movieId = "movie1";

        
        _bookmarkRepositoryMock.Setup(repo => repo.BookmarkExistsAsync(userId, movieId))
                               .ReturnsAsync(false); // Bookmark does not exist

        // Act
        var result = await _controller.RemoveBookmark(userId, movieId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Bookmark does not exist.", notFoundResult.Value);
    }


}