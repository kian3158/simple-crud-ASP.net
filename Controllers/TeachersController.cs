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

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Courses)
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber
                })
                .ToListAsync();

            return Ok(teachers);
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherDto>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Courses)
                .Where(t => t.Id == id)
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber
                })
                .FirstOrDefaultAsync();

            if (teacher == null) return NotFound();
            return Ok(teacher);
        }

        // POST: api/Teachers
        [HttpPost]
        public async Task<ActionResult<TeacherDto>> PostTeacher(TeacherDto teacherDto)
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

        // PUT: api/Teachers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, TeacherDto teacherDto)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            teacher.Name = teacherDto.Name;
            teacher.Email = teacherDto.Email;
            teacher.PhoneNumber = teacherDto.PhoneNumber;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
