using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.Dtos;
using SchoolApi.Services;

namespace SchoolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService) => _courseService = courseService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _courseService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateUpdateDto dto)
        {
            var id = await _courseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseCreateUpdateDto dto)
        {
            await _courseService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteAsync(id);
            return NoContent();
        }
    }
}
