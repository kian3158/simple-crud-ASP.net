using SchoolApi.Dtos;

namespace SchoolApi.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CourseCreateUpdateDto dto);
        Task UpdateAsync(int id, CourseCreateUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
