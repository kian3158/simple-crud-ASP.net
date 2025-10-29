using SchoolApi.Dtos;

namespace SchoolApi.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllAsync();
        Task<StudentDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(StudentDto dto);
        Task UpdateAsync(int id, StudentDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<CourseDto>> GetCoursesByStudentEmailAsync(string email);
    }
}
