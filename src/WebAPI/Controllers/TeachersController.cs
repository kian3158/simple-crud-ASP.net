using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    [Authorize(Roles = "Admin,Teacher")]
    public class TeachersController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeachersController(SchoolContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Admin: get all teachers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _context.Teachers
                .Select(t => new { t.Id, t.Name, t.Email, t.PhoneNumber })
                .ToListAsync();
            return Ok(teachers);
        }

        // Admin: create teacher
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(TeacherCreateUpdateDto dto)
        {
            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, "Teacher");

            var teacher = new Teacher
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                ApplicationUserId = user.Id
            };
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return Ok(new { teacher.Id, teacher.Name, teacher.Email, teacher.PhoneNumber });
        }

        // Admin: update teacher
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, TeacherCreateUpdateDto dto)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            teacher.Name = dto.Name;
            teacher.Email = dto.Email;
            teacher.PhoneNumber = dto.PhoneNumber;
            await _context.SaveChangesAsync();

            return Ok(new { teacher.Id, teacher.Name, teacher.Email, teacher.PhoneNumber });
        }

        // Admin: delete teacher
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Teacher: view own profile
        [HttpGet("me")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacher = await _context.Teachers
                .Where(t => t.ApplicationUserId == userId)
                .Select(t => new { t.Id, t.Name, t.Email, t.PhoneNumber })
                .FirstOrDefaultAsync();

            if (teacher == null) return NotFound();
            return Ok(teacher);
        }
    }
}
