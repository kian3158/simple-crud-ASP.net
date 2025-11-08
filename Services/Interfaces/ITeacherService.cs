using SchoolApi.Dtos;

namespace SchoolApi.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllAsync();
        Task<TeacherDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(TeacherCreateUpdateDto dto);
        Task UpdateAsync(int id, TeacherCreateUpdateDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<CourseDto>> GetCoursesByTeacherEmailAsync(string email);
    }
}
