using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;

        public CourseService(SchoolContext context)
        {
            _context = context;
        }

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
            var c = await _context.Courses
                .AsNoTracking()
                .Where(x => x.CourseId == id)
                .Select(x => new CourseDto
                {
                    CourseId = x.CourseId,
                    CourseName = x.CourseName,
                    TeacherId = x.TeacherId
                })
                .FirstOrDefaultAsync();

            return c;
        }

        public async Task<int> CreateAsync(CourseDto dto)
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

        public async Task UpdateAsync(int id, CourseDto dto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) throw new KeyNotFoundException($"Course {id} not found.");

            // update allowed fields only
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
