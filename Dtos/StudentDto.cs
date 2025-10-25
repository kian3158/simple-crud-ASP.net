namespace SchoolApi.Dtos
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public List<CourseDto>? Courses { get; set; }
    }
}
