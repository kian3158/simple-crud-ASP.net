namespace SchoolApi.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class StudentDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^\+?[0-9\-\s]{7,20}$", ErrorMessage = "Phone number is invalid.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
