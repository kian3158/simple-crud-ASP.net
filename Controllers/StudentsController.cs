using Microsoft.AspNetCore.Mvc;
using SchoolApi.Dtos;
using SchoolApi.Services;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _svc;
        public StudentsController(IStudentService svc) => _svc = svc;

        // GET: api/Students
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _svc.GetAllAsync();
            return Ok(list);
        }

        // GET: api/Students/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var s = await _svc.GetByIdAsync(id);
            if (s == null) return NotFound();
            return Ok(s);
        }

        // GET: api/Students/courses-by-email/{email}
        [HttpGet("courses-by-email/{email}")]
        public async Task<IActionResult> GetCoursesByEmail(string email)
        {
            var courses = await _svc.GetCoursesByStudentEmailAsync(email);
            return Ok(courses); // returns empty list if none
        }

        // POST: api/Students
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentDto dto)
        {
            // [ApiController] auto-validates DTO and returns 400 if invalid
            var id = await _svc.CreateAsync(dto);
            dto.Id = id;
            return CreatedAtAction(nameof(Get), new { id }, dto);
        }

        // PUT: api/Students/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentDto dto)
        {
            try
            {
                await _svc.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Students/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _svc.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
