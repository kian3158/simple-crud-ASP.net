using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;
using SchoolApi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly SchoolContext _context;

        public TeacherService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherDto>> GetAllAsync()
        {
            return await _context.Teachers
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber
                })
                .ToListAsync();
        }

        public async Task<TeacherDto?> GetByIdAsync(int id)
        {
            return await _context.Teachers
                .Where(t => t.Id == id)
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TeacherDto> CreateAsync(TeacherDto dto)
        {
            var entity = new Teacher
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            _context.Teachers.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, TeacherDto dto)
        {
            var entity = await _context.Teachers.FindAsync(id);
            if (entity == null) return false;

            entity.Name = dto.Name;
            entity.Email = dto.Email;
            entity.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Teachers.FindAsync(id);
            if (entity == null) return false;

            _context.Teachers.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
