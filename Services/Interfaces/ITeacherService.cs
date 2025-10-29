using SchoolApi.Dtos;

namespace SchoolApi.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllAsync();
        Task<TeacherDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(TeacherDto dto);
        Task UpdateAsync(int id, TeacherDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<CourseDto>> GetCoursesByTeacherEmailAsync(string email);
    }
}
