using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Get all users with their roles
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList()
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email
                }).ToList();

            return Ok(users);
        }

        // Change user role
        [HttpPost("users/{id}/role")]
        public async Task<IActionResult> ChangeUserRole(string id, [FromBody] string role)
        {
            if (!new[] { "Admin", "Teacher", "Student" }.Contains(role))
                return BadRequest("Invalid role");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);

            return Ok(new { user.Id, user.UserName, NewRole = role });
        }
    }
}
