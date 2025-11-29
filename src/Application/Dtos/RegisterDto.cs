using System.ComponentModel.DataAnnotations;

namespace SchoolApi.Application.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Phone must be a valid international phone number (digits, optional leading +).")]
        public string PhoneNumber { get; set; } = null!;

        public string Role { get; set; } = "Student";
    }
}
