namespace SchoolApi.Dtos
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public List<StudentDto>? Students { get; set; }
    }
}
