using BCrypt.Net;
using finalesYaBackend.DTOs;
using finalesYaBackend.Services;
using finalesYaBackend.Models;

namespace finalesYaBackend.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Simula operación asíncrona
            await Task.Delay(1);
            return _users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            // Simula operación asíncrona
            await Task.Delay(1);
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User> CreateAsync(UserCreateDto dto)
        {
            await Task.Delay(1);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Id = _nextId++,
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                University = dto.University,
                Role = dto.Role,
                RegisteredAt = DateTime.UtcNow
            };

            _users.Add(user);
            return user;
        }


        public async Task<User?> UpdateAsync(int id, User updatedUser)
        {
            // Simula operación asíncrona
            await Task.Delay(1);
            
            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                return null; // Retorna null si no existe
            }

            existing.Name = updatedUser.Name;
            existing.Email = updatedUser.Email;
            existing.PasswordHash = updatedUser.PasswordHash;
            existing.University = updatedUser.University;
            existing.Role = updatedUser.Role;
            
            return existing; // Retorna el usuario actualizado
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Simula operación asíncrona
            await Task.Delay(1);
            
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false; // Retorna false si no existe
            }

            _users.Remove(user);
            return true; // Retorna true si se eliminó correctamente
        }
    }
}