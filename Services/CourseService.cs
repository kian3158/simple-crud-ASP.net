using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;
using SchoolApi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .Where(c => c.CourseId == id)
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherId = c.TeacherId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CourseDto> CreateAsync(CourseDto dto)
        {
            var entity = new Course
            {
                CourseName = dto.CourseName,
                TeacherId = dto.TeacherId
            };

            _context.Courses.Add(entity);
            await _context.SaveChangesAsync();

            dto.CourseId = entity.CourseId;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, CourseDto dto)
        {
            var entity = await _context.Courses.FindAsync(id);
            if (entity == null) return false;

            entity.CourseName = dto.CourseName;
            entity.TeacherId = dto.TeacherId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Courses.FindAsync(id);
            if (entity == null) return false;

            _context.Courses.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
