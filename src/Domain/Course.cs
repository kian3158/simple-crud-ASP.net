namespace Domain
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;

        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public ICollection<StudentCourse>? StudentCourses { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();

    }
}
