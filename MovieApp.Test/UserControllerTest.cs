using DataAccessLayer.DataModels;
using DataAccessLayer.Repository.UserRepository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SUbProject_02_MovieApp.Controllers;
using SUbProject_02_MovieApp.DTOModels.UserDTOs;

namespace MovieApp.Test
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserController _userController;

        public UserControllerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userController = new UserController(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Register_ValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var controller = new UserController(mockUserRepo.Object);

            string userId = "123";
            string username = "testuser";
            string email = "test@example.com";
            string password = "password";

            mockUserRepo.Setup(repo => repo.getUserbyUsername(username))
                        .ReturnsAsync((user_info)null);  // Simulate no existing user with this username

            mockUserRepo.Setup(repo => repo.AddUser(It.IsAny<user_info>()))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await controller.RegisterUser(userId, username, email, password);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var registeredUser = Assert.IsType<user_info>(okResult.Value);
            Assert.Equal(userId, registeredUser.user_id);
            Assert.Equal(username, registeredUser.username);
            Assert.Equal(email, registeredUser.email);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            string username = "invalidUser";
            string password = "wrongPassword";

            _userRepositoryMock.Setup(repo => repo.LoginUser(username, password))
                .ReturnsAsync((user_info)null);  

            // Act
            var result = await _userController.Login(username, password);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password.", unauthorizedResult.Value);
        }


    }
}
