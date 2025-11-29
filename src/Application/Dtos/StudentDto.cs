using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolApi.Application.Dtos
{
    public class StudentDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Phone must be a valid international phone number (digits, optional leading +).")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
