using System.Threading.Tasks;
using SchoolApi.Dtos;

namespace SchoolApi.Services
{
    public interface IAuthService
    {
        Task<(bool Succeeded, string? Error)> RegisterAsync(RegisterDto dto);
        Task<(bool Succeeded, string? Token, string? Error)> LoginAsync(LoginDto dto);
    }
}
