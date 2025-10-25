using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly SchoolContext _context;

        public TeachersController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAllTeachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Courses)
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber,
                    Courses = t.Courses.Select(c => new CourseDto
                    {
                        CourseId = c.CourseId,
                        CourseName = c.CourseName,
                        TeacherId = c.TeacherId
                    }).ToList()
                }).ToListAsync();

            return Ok(teachers);
        }

        // GET: api/teachers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherDto>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return NotFound();

            var teacherDto = new TeacherDto
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber,
                Courses = teacher.Courses.Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherId = c.TeacherId
                }).ToList()
            };

            return Ok(teacherDto);
        }

        // POST: api/teachers
        [HttpPost]
        public async Task<ActionResult<TeacherDto>> CreateTeacher(TeacherDto teacherDto)
        {
            var teacher = new Teacher
            {
                Name = teacherDto.Name,
                Email = teacherDto.Email,
                PhoneNumber = teacherDto.PhoneNumber
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            teacherDto.Id = teacher.Id;
            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, teacherDto);
        }
    }
}
