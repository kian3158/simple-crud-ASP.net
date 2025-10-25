namespace SchoolApi.Models
{
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty; 
    public ICollection<StudentCourse>? StudentCourses { get; set; }
}
}
