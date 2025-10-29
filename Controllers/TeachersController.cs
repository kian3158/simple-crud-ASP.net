using Microsoft.AspNetCore.Mvc;
using SchoolApi.Dtos;
using SchoolApi.Services;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _svc;
        public TeachersController(ITeacherService svc) => _svc = svc;

        // GET: api/Teachers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _svc.GetAllAsync();
            return Ok(list);
        }

        // GET: api/Teachers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var t = await _svc.GetByIdAsync(id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        // GET: api/Teachers/courses-by-email/{email}
        [HttpGet("courses-by-email/{email}")]
        public async Task<IActionResult> GetCoursesByEmail(string email)
        {
            var courses = await _svc.GetCoursesByTeacherEmailAsync(email);
            return Ok(courses);
        }

        // POST: api/Teachers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherDto dto)
        {
            var id = await _svc.CreateAsync(dto);
            dto.Id = id;
            return CreatedAtAction(nameof(Get), new { id }, dto);
        }

        // PUT: api/Teachers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeacherDto dto)
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

        // DELETE: api/Teachers/{id}
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
