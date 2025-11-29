using Microsoft.EntityFrameworkCore;
using SchoolApi.Infrastructure;
using SchoolApi.Application.Dtos;
using SchoolApi.Domain.Models;

namespace SchoolApi.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;
        public CourseService(SchoolContext context) => _context = context;

        public async Task<IEnumerable<CourseDto>> GetAllAsync()
        {
            return await _context.Courses
                .AsNoTracking()
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherId = c.TeacherId
                })
                .ToListAsync();
        }

        public async Task<CourseDto?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .AsNoTracking()
                .Where(c => c.CourseId == id)
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherId = c.TeacherId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CourseCreateUpdateDto dto)
        {
            var course = new Course
            {
                CourseName = dto.CourseName,
                TeacherId = dto.TeacherId
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course.CourseId;
        }

        public async Task UpdateAsync(int id, CourseCreateUpdateDto dto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) throw new KeyNotFoundException($"Course {id} not found.");
            course.CourseName = dto.CourseName;
            course.TeacherId = dto.TeacherId;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) throw new KeyNotFoundException($"Course {id} not found.");
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
