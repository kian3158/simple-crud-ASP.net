using System.ComponentModel.DataAnnotations;

public class CourseCreateUpdateDto
{
    [Required]
    public string CourseName { get; set; } = string.Empty;

    [Required]
    public int TeacherId { get; set; }
}
