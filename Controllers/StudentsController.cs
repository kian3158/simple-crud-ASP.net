using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    DateOfBirth = s.DateOfBirth
                })
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .Where(s => s.Id == id)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    DateOfBirth = s.DateOfBirth
                })
                .FirstOrDefaultAsync();

            if (student == null) return NotFound();
            return Ok(student);
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudent(StudentDto studentDto)
        {
            var student = new Student
            {
                Name = studentDto.Name,
                Email = studentDto.Email,
                PhoneNumber = studentDto.PhoneNumber,
                DateOfBirth = studentDto.DateOfBirth
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            studentDto.Id = student.Id;
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, studentDto);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentDto studentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            student.Name = studentDto.Name;
            student.Email = studentDto.Email;
            student.PhoneNumber = studentDto.PhoneNumber;
            student.DateOfBirth = studentDto.DateOfBirth;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
