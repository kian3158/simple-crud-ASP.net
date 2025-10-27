using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolApi.Dtos;

namespace SchoolApi.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllAsync();
        Task<StudentDto?> GetByIdAsync(int id);
        Task<StudentDto> CreateAsync(StudentDto dto);
        Task<bool> UpdateAsync(int id, StudentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
