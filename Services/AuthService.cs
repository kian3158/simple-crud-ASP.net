using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<(bool Succeeded, string? Error)> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return (false, "Email already in use");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var err = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, err);
            }

            return (true, null);
        }

        public async Task<(bool Succeeded, string? Token, string? Error)> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return (false, null, "Invalid credentials");

            var res = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!res.Succeeded) return (false, null, "Invalid credentials");

            var token = await GenerateJwtToken(user);
            return (true, token, null);
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var keyString = jwtSection.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key not configured");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expiresMinutes = jwtSection.GetValue<int?>("ExpireMinutes")
                                ?? jwtSection.GetValue<int?>("ExpiresMinutes")
                                ?? 60;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }.ToList();

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
