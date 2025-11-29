using Microsoft.EntityFrameworkCore;
using SchoolApi.Infrastructure;
using SchoolApi.Application.Dtos;

namespace SchoolApi.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly SchoolContext _context;
        public TeacherService(SchoolContext context) => _context = context;

        public async Task<IEnumerable<TeacherDto>> GetAllAsync()
        {
            return await _context.Teachers
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber
                })
                .ToListAsync();
        }

        public async Task<TeacherDto?> GetByIdAsync(int id)
        {
            return await _context.Teachers
                .Where(t => t.Id == id)
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(TeacherCreateUpdateDto dto)
        {
            var t = new SchoolApi.Domain.Models.Teacher
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            _context.Teachers.Add(t);
            await _context.SaveChangesAsync();
            return t.Id;
        }

        public async Task UpdateAsync(int id, TeacherCreateUpdateDto dto)
        {
            var t = await _context.Teachers.FindAsync(id);
            if (t == null) throw new KeyNotFoundException("Teacher not found");
            t.Name = dto.Name;
            t.Email = dto.Email;
            t.PhoneNumber = dto.PhoneNumber;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var t = await _context.Teachers.FindAsync(id);
            if (t == null) throw new KeyNotFoundException("Teacher not found");
            _context.Teachers.Remove(t);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByTeacherEmailAsync(string email)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Where(c => c.Teacher != null && c.Teacher.Email == email)
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherId = c.TeacherId
                })
                .ToListAsync();
        }
    }
}
