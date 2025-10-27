using Microsoft.AspNetCore.Mvc;
using SchoolApi.Dtos;
using SchoolApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachers()
        {
            var teachers = await _teacherService.GetAllAsync();
            return Ok(teachers);
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherDto>> GetTeacher(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);
            if (teacher == null) return NotFound();
            return Ok(teacher);
        }

        // POST: api/Teachers
        [HttpPost]
        public async Task<ActionResult<TeacherDto>> PostTeacher([FromBody] TeacherDto teacherDto)
        {
            var created = await _teacherService.CreateAsync(teacherDto);
            return CreatedAtAction(nameof(GetTeacher), new { id = created.Id }, created);
        }

        // PUT: api/Teachers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, [FromBody] TeacherDto teacherDto)
        {
            var updated = await _teacherService.UpdateAsync(id, teacherDto);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var deleted = await _teacherService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
