using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SUbProject_02_MovieApp.Controllers;
using DataAccessLayer.Repository.NewFolder;
using System.Threading.Tasks;

public class NotesControllerTests
{
    private readonly Mock<INotesRepository> _notesRepositoryMock;
    private readonly NotesController _controller;

    public NotesControllerTests()
    {
        _notesRepositoryMock = new Mock<INotesRepository>();
        _controller = new NotesController(_notesRepositoryMock.Object, null);
    }

    [Fact]
    public async Task AddNote_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        string userId = "user123";
        string movieId = "movie456";
        string noteContent = "This is a note";

        
        _notesRepositoryMock.Setup(repo => repo.UserExistsCheck(userId)).ReturnsAsync(false);

        // Act
        var result = await _controller.AddNote(userId, movieId, noteContent);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"User with ID {userId} does not exist.", notFoundResult.Value);
    }
    [Fact]
    public async Task DeleteNote_ReturnsNotFound_WhenNoteDoesNotExist()
    {
        // Arrange
        string userId = "user123";
        string movieId = "movie456";

        // Simulate that the note does not exist in the repository
        _notesRepositoryMock.Setup(repo => repo.DeleteNotes(userId, movieId))
                            .ThrowsAsync(new KeyNotFoundException("Note not found"));

        // Act
        var result = await _controller.DeleteNote(userId, movieId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Note not found for the specified user and movie.", notFoundResult.Value);
    }


}
