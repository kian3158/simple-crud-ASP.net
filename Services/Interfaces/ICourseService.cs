using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolApi.Dtos;

namespace SchoolApi.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CourseDto dto);
        Task<bool> UpdateAsync(int id, CourseDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
