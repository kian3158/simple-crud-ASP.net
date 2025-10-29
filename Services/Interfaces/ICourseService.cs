using SchoolApi.Dtos;

namespace SchoolApi.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CourseDto dto);
        Task UpdateAsync(int id, CourseDto dto);
        Task DeleteAsync(int id);
    }
}
