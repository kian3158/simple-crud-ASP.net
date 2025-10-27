using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolApi.Dtos;

namespace SchoolApi.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllAsync();
        Task<TeacherDto?> GetByIdAsync(int id);
        Task<TeacherDto> CreateAsync(TeacherDto dto);
        Task<bool> UpdateAsync(int id, TeacherDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
