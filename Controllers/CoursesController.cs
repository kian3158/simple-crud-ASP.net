using Microsoft.AspNetCore.Mvc;
using SchoolApi.Dtos;
using SchoolApi.Services;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _svc;
        public CoursesController(ICourseService svc) => _svc = svc;

        // GET: api/Courses
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _svc.GetAllAsync();
            return Ok(list);
        }

        // GET: api/Courses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var c = await _svc.GetByIdAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseDto dto)
        {
            var id = await _svc.CreateAsync(dto);
            dto.CourseId = id;
            return CreatedAtAction(nameof(Get), new { id }, dto);
        }

        // PUT: api/Courses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseDto dto)
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

        // DELETE: api/Courses/{id}
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
