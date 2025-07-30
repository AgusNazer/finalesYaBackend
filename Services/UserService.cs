using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using finalesYaBackend.DTOs;
using finalesYaBackend.Services;
using finalesYaBackend.Models;
// using finalesYaBackend.Data;

namespace finalesYaBackend.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateAsync(UserCreateDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                University = dto.University,
                Role = dto.Role,
                RegisteredAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();  // ← ESTO guarda en la DB real

            return user;
        }

        public async Task<User?> UpdateAsync(int id, User updatedUser)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
            {
                return null;
            }

            existing.Name = updatedUser.Name;
            existing.Email = updatedUser.Email;
            existing.PasswordHash = updatedUser.PasswordHash;
            existing.University = updatedUser.University;
            existing.Role = updatedUser.Role;
            
            await _context.SaveChangesAsync();  // ← Guardar cambios
            
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();  // ← Guardar cambios
            
            return true;
        }
    }
}