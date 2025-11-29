using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Infrastructure;
using SchoolApi.Application.Dtos;
using SchoolApi.Domain.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolApi.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class CoursesController : ControllerBase
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        // Admin: get all courses
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Courses.Include(c => c.Teacher).ToListAsync();
            var dtos = courses.Select(c => new { c.CourseId, c.CourseName, c.TeacherId }).ToList();
            return Ok(dtos);
        }

        // Teacher: get my courses
        [HttpGet("my")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacher = await _context.Teachers.Include(t => t.Courses).FirstOrDefaultAsync(t => t.ApplicationUserId == userId);
            if (teacher == null) return NotFound();

            var courses = teacher.Courses.Select(c => new { c.CourseId, c.CourseName }).ToList();
            return Ok(courses);
        }

        // Student: get my courses
        [HttpGet("my-student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyCoursesForStudent()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = await _context.Students
                .Include(s => s.StudentCourses!)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

            if (student == null) return NotFound();

            var courses = student.StudentCourses
                .Select(sc => new { sc.Course!.CourseId, sc.Course.CourseName, sc.Course.TeacherId })
                .ToList();

            return Ok(courses);
        }

        // Admin: create course
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CourseCreateUpdateDto dto)
        {
            var course = new Course { CourseName = dto.CourseName, TeacherId = dto.TeacherId };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return Ok(new { course.CourseId, course.CourseName, course.TeacherId });
        }

        // Admin/Teacher: update course
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(int id, CourseCreateUpdateDto dto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            if (User.IsInRole("Teacher"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.ApplicationUserId == userId);
                if (teacher == null || course.TeacherId != teacher.Id)
                    return Forbid();
            }

            course.CourseName = dto.CourseName;
            await _context.SaveChangesAsync();
            return Ok(new { course.CourseId, course.CourseName });
        }

        // Admin: delete course
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Admin/Teacher/Student: get students by course
        [HttpGet("{id}/students")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetStudentsByCourse(int id)
        {
            var course = await _context.Courses.Include(c => c.StudentCourses!).ThenInclude(sc => sc.Student!).FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null) return NotFound();

            if (User.IsInRole("Teacher"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.ApplicationUserId == userId);
                if (teacher == null || course.TeacherId != teacher.Id)
                    return Forbid();
            }

            if (User.IsInRole("Student"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!course.StudentCourses.Any(sc => sc.Student!.ApplicationUserId == userId))
                    return Forbid();
            }

            var students = course.StudentCourses
                .Select(sc => new { sc.Student!.Id, sc.Student.Name, sc.Student.Email, sc.Student.DateOfBirth, sc.Student.PhoneNumber })
                .ToList();

            return Ok(students);
        }
    }
}
