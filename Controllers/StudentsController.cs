using Microsoft.AspNetCore.Mvc;
using SchoolApi.Dtos;
using SchoolApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudent([FromBody] StudentDto studentDto)
        {
            var created = await _studentService.CreateAsync(studentDto);
            return CreatedAtAction(nameof(GetStudent), new { id = created.Id }, created);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, [FromBody] StudentDto studentDto)
        {
            var updated = await _studentService.UpdateAsync(id, studentDto);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var deleted = await _studentService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
