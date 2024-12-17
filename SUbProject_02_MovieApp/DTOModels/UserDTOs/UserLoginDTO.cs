using System.ComponentModel.DataAnnotations;

namespace SUbProject_02_MovieApp.DTOModels.UserDTOs
{
    public class UserLoginDTO
    {
        [Required]
        public string Identifier { get; set; } // Accepts either username or email.

        [Required]
        public string Password { get; set; }
    }
}

