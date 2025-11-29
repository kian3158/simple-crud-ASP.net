using System.Threading.Tasks;
using SchoolApi.Application.Dtos;

namespace SchoolApi.Application.Services
{
    public interface IAuthService
    {
        Task<(bool Succeeded, string? Error)> RegisterAsync(RegisterDto dto);
        Task<(bool Succeeded, string? Token, string? Error)> LoginAsync(LoginDto dto);
    }
}
