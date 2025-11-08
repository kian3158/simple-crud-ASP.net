using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;
        public StudentService(SchoolContext context) => _context = context;

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            return await _context.Students
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    DateOfBirth = s.DateOfBirth
                })
                .ToListAsync();
        }

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    DateOfBirth = s.DateOfBirth
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(StudentCreateUpdateDto dto)
        {
            var s = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth
            };
            _context.Students.Add(s);
            await _context.SaveChangesAsync();
            return s.Id;
        }

        public async Task UpdateAsync(int id, StudentCreateUpdateDto dto)
        {
            var s = await _context.Students.FindAsync(id);
            if (s == null) throw new KeyNotFoundException("Student not found");
            s.Name = dto.Name;
            s.Email = dto.Email;
            s.PhoneNumber = dto.PhoneNumber;
            s.DateOfBirth = dto.DateOfBirth;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var s = await _context.Students.FindAsync(id);
            if (s == null) throw new KeyNotFoundException("Student not found");
            _context.Students.Remove(s);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByStudentEmailAsync(string email)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Course)
                .Include(sc => sc.Student)
                .Where(sc => sc.Student.Email == email)
                .Select(sc => new CourseDto
                {
                    CourseId = sc.Course.CourseId,
                    CourseName = sc.Course.CourseName,
                    TeacherId = sc.Course.TeacherId
                })
                .ToListAsync();
        }
    }
}
