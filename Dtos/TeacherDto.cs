namespace SchoolApi.Dtos
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public List<CourseDto>? Courses { get; set; }
    }
}
