using SchoolApi.Application.Dtos;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(StudentCreateUpdateDto dto); 
    Task UpdateAsync(int id, StudentCreateUpdateDto dto); 
    Task DeleteAsync(int id);
    Task<IEnumerable<CourseDto>> GetCoursesByStudentEmailAsync(string email);
}
