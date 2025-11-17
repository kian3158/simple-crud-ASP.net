using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Student,Teacher")]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // Admin: get all students
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var students = await _context.Students
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Email,
                    s.DateOfBirth,
                    s.PhoneNumber
                })
                .ToListAsync();

            return Ok(students);
        }

        // Admin: create student
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(StudentCreateUpdateDto dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                PhoneNumber = dto.PhoneNumber
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                student.Id,
                student.Name,
                student.Email,
                student.DateOfBirth,
                student.PhoneNumber
            });
        }

        // Admin: update student
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, StudentCreateUpdateDto dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            student.Name = dto.Name;
            student.Email = dto.Email;
            student.DateOfBirth = dto.DateOfBirth;
            student.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                student.Id,
                student.Name,
                student.Email,
                student.DateOfBirth,
                student.PhoneNumber
            });
        }

        // Admin: delete student
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Student: view own profile
        [HttpGet("me")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = await _context.Students
                .Where(s => s.ApplicationUserId == userId)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Email,
                    s.DateOfBirth,
                    s.PhoneNumber
                })
                .FirstOrDefaultAsync();

            if (student == null) return NotFound();
            return Ok(student);
        }
    }
}
