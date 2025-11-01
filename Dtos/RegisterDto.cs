using System.ComponentModel.DataAnnotations;

namespace SchoolApi.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
