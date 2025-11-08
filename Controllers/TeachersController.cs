using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.Dtos;
using SchoolApi.Services;


namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        public TeachersController(ITeacherService teacherService) => _teacherService = teacherService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _teacherService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);
            if (teacher == null) return NotFound();
            return Ok(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherCreateUpdateDto dto)
        {
            var id = await _teacherService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeacherCreateUpdateDto dto)
        {
            await _teacherService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("courses/{email}")]
        public async Task<IActionResult> GetCoursesByEmail(string email)
        {
            var courses = await _teacherService.GetCoursesByTeacherEmailAsync(email);
            return Ok(courses);
        }
    }
}
