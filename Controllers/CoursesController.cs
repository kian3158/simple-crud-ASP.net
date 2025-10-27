using Microsoft.AspNetCore.Mvc;
using SchoolApi.Dtos;
using SchoolApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> PostCourse([FromBody] CourseDto courseDto)
        {
            var created = await _courseService.CreateAsync(courseDto);
            return CreatedAtAction(nameof(GetCourse), new { id = created.CourseId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, [FromBody] CourseDto courseDto)
        {
            var updated = await _courseService.UpdateAsync(id, courseDto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var deleted = await _courseService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
