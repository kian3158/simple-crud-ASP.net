using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolApi.Application.Dtos
{
    public class TeacherDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Phone must be a valid international phone number (digits, optional leading +).")]
        public string PhoneNumber { get; set; } = null!;

        public List<CourseDto>? Courses { get; set; }
    }
}
