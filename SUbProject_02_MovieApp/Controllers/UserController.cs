
using DataAccessLayer.DataModels;
using DataAccessLayer.Repository.UserRepository;
using Microsoft.AspNetCore.Mvc;
using SUbProject_02_MovieApp.DTOModels.UserDTOs;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SUbProject_02_MovieApp.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data.");

            // Validate if Username or Email already exists
            var existingUserByUsername = await _userRepository.getUserbyUsername(registerDTO.UserName);
            if (existingUserByUsername != null)
                return Conflict(new { message = "The username is already in use." });

            var existingUserByEmail = await _userRepository.getUserByEmail(registerDTO.Email);
            if (existingUserByEmail != null)
                return Conflict(new { message = "The email address is already registered." });

            // Auto-generate UserId and hash the password
            var userId = Guid.NewGuid().ToString();
            var hashedPassword = HashPassword(registerDTO.Password);

            // Create the user object
            var user = new user_info
            {
                user_id = userId,
                username = registerDTO.UserName,
                email = registerDTO.Email,
                user_psw = hashedPassword,
                created_date = DateTime.UtcNow
            };

            // Save user to the database
            await _userRepository.AddUser(user);

            return Ok(new { message = "User registered successfully.", userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login data.");

            // Fetch user by identifier
            var user = await _userRepository.getUserByIdentifier(loginDTO.Identifier);

            if (user == null || !VerifyPassword(loginDTO.Password, user.user_psw))
                return Unauthorized(new { message = "Invalid username/email or password." });

            return Ok(new { message = "Login successful.", userId = user.user_id });
        }

        [HttpGet("userinfo/{userId}")]
        public async Task<IActionResult> GetUserInfo(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new { message = "User ID cannot be empty." });

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                return NotFound(new { message = $"User with ID {userId} not found." });

            var userInfo = new
            {
                Username = user.username,
                Email = user.email
            };

            return Ok(userInfo);
        }


        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserRegisterDTO updateDTO)
        {
            var existingUser = await _userRepository.getUserbyUsername(userId);
            if (existingUser == null)
                return NotFound(new { message = "User not found." });

            // Update user details
            existingUser.username = updateDTO.UserName ?? existingUser.username;
            existingUser.email = updateDTO.Email ?? existingUser.email;

            if (!string.IsNullOrEmpty(updateDTO.Password))
                existingUser.user_psw = HashPassword(updateDTO.Password);

            await _userRepository.UpdateUser(existingUser);

            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userRepository.getUserbyUsername(userId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            await _userRepository.DeleteUser(userId);

            return Ok(new { message = "User deleted successfully." });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string password, string storedPasswordHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedInputPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return hashedInputPassword == storedPasswordHash;
            }
        }
    }
}

