using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll()
        {
            var students = await _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .ToListAsync();

            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                DateOfBirth = s.DateOfBirth,
                PhoneNumber = s.PhoneNumber,
                Courses = s.StudentCourses?.Select(sc => new CourseDto
                {
                    CourseId = sc.Course.CourseId,
                    CourseName = sc.Course.CourseName,
                    TeacherId = sc.Course.TeacherId
                }).ToList()
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetById(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                PhoneNumber = student.PhoneNumber,
                Courses = student.StudentCourses?.Select(sc => new CourseDto
                {
                    CourseId = sc.Course.CourseId,
                    CourseName = sc.Course.CourseName,
                    TeacherId = sc.Course.TeacherId
                }).ToList()
            };
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create(StudentDto studentDto)
        {
            var student = new Student
            {
                Name = studentDto.Name,
                Email = studentDto.Email,
                DateOfBirth = studentDto.DateOfBirth,
                PhoneNumber = studentDto.PhoneNumber
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            studentDto.Id = student.Id;
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, studentDto);
        }
    }
}
