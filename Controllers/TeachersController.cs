using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly SchoolContext _context;

        public TeachersController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Courses)
                .ToListAsync();

            var teacherDtos = teachers.Select(t => new TeacherDto
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Courses = t.Courses?.Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName
                }).ToList()
            }).ToList();

            return Ok(teacherDtos);
        }
    }
}
