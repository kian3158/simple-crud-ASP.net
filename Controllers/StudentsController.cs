using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.Dtos;
using SchoolApi.Services;


namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService) => _studentService = studentService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _studentService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentCreateUpdateDto dto)
        {
            var id = await _studentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentCreateUpdateDto dto)
        {
            await _studentService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("courses/{email}")]
        public async Task<IActionResult> GetCoursesByEmail(string email)
        {
            var courses = await _studentService.GetCoursesByStudentEmailAsync(email);
            return Ok(courses);
        }
    }
}
