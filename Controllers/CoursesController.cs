using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Students)
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherId = c.TeacherId,
                    Students = c.Students.Select(s => new StudentDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Email = s.Email,
                        DateOfBirth = s.DateOfBirth,
                        PhoneNumber = s.PhoneNumber
                    }).ToList()
                }).ToListAsync();

            return Ok(courses);
        }

        // GET: api/courses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
                return NotFound();

            var courseDto = new CourseDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                TeacherId = course.TeacherId,
                Students = course.Students.Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    DateOfBirth = s.DateOfBirth,
                    PhoneNumber = s.PhoneNumber
                }).ToList()
            };

            return Ok(courseDto);
        }

        // POST: api/courses
        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseDto courseDto)
        {
            var course = new Course
            {
                CourseName = courseDto.CourseName,
                TeacherId = courseDto.TeacherId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            courseDto.CourseId = course.CourseId;
            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, courseDto);
        }
    }
}
