using System.ComponentModel.DataAnnotations;
using WebServer.Models.Entities;

namespace WebServer.Models.DTO
{
    public class UserDto
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        //public UserStatus Status { get; set; } // Options: Active, Blocked, Unverified

    }
}
