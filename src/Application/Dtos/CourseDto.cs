namespace SchoolApi.Application.Dtos
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public int TeacherId { get; set; }

    }
}
