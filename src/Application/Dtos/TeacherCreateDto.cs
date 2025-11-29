using System.ComponentModel.DataAnnotations;

public class TeacherCreateUpdateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

   //[Required, RegularExpression(@"^(\+98|0)?9\d{9}$", ErrorMessage = "Invalid Iranian phone number")]
    [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Phone must be a valid international phone number (digits, optional leading +).")]

    public string PhoneNumber { get; set; } = string.Empty;
}
