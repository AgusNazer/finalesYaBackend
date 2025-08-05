// using BCrypt.Net;
// using Microsoft.EntityFrameworkCore;
// using finalesYaBackend.DTOs;
// using finalesYaBackend.Services;
// using finalesYaBackend.Models;
// // using finalesYaBackend.Data;
//
// namespace finalesYaBackend.Services
// {
//     public class UserService : IUserService
//     {
//         private readonly AppDbContext _context;
//
//         public UserService(AppDbContext context)
//         {
//             _context = context;
//         }
//
//         public async Task<IEnumerable<UserReadDto>> GetAllAsync()
//         {
//             var users = await _context.Users.ToListAsync();
//     
//             return users.Select(user => new UserReadDto
//             {
//                 Id = user.Id,
//                 Name = user.Name,
//                 Email = user.Email,
//                 University = user.University,
//                 Role = user.Role,
//                 RegisteredAt = user.RegisteredAt
//             });
//         }
//
//         public async Task<UserReadDto?> GetByIdAsync(int id)
//         {
//             var user = await _context.Users.FindAsync(id);
//             if (user == null) return null;
//     
//             return new UserReadDto
//             {
//                 Id = user.Id,
//                 Name = user.Name,
//                 Email = user.Email,
//                 University = user.University,
//                 Role = user.Role,
//                 RegisteredAt = user.RegisteredAt
//             };
//         }
//
//         public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
//         {
//             var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
//
//             var user = new User
//             {
//                 Name = dto.Name,
//                 Email = dto.Email,
//                 PasswordHash = hashedPassword,
//                 University = dto.University,
//                 Role = dto.Role,
//                 RegisteredAt = DateTime.UtcNow
//             };
//
//             _context.Users.Add(user);
//             await _context.SaveChangesAsync();
//
//             // Devolver UserReadDto en lugar de User
//             return new UserReadDto
//             {
//                 Id = user.Id,
//                 Name = user.Name,
//                 Email = user.Email,
//                 University = user.University,
//                 Role = user.Role,
//                 RegisteredAt = user.RegisteredAt
//             };
//         }
//
//         public async Task<User?> UpdateAsync(int id, User updatedUser)
//         {
//             var existing = await _context.Users.FindAsync(id);
//             if (existing == null)
//             {
//                 return null;
//             }
//
//             existing.Name = updatedUser.Name;
//             existing.Email = updatedUser.Email;
//             existing.PasswordHash = updatedUser.PasswordHash;
//             existing.University = updatedUser.University;
//             existing.Role = updatedUser.Role;
//             
//             await _context.SaveChangesAsync();  // ← Guardar cambios
//             
//             return existing;
//         }
//
//         public async Task<bool> DeleteAsync(int id)
//         {
//             var user = await _context.Users.FindAsync(id);
//             if (user == null)
//             {
//                 return false;
//             }
//
//             _context.Users.Remove(user);
//             await _context.SaveChangesAsync();  // ← Guardar cambios
//             
//             return true;
//         }
//     }
// }

using finalesYaBackend.DTOs;
using finalesYaBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace finalesYaBackend.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Usuario> _userManager;

        public UserService(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllAsync()
        {
            Console.WriteLine("🔍 Llegó al controller GetAll()");
            var users = await _userManager.Users.ToListAsync();

            return users.Select(user => new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                University = user.University,
                Role = user.Role,
                RegisteredAt = user.RegisteredAt
            });
        }

        public async Task<UserReadDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                University = user.University,
                Role = user.Role,
                RegisteredAt = user.RegisteredAt
            };
        }

        public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
        {
            var user = new Usuario
            {
                UserName = dto.Email, // Identity requiere esto
                Email = dto.Email,
                Name = dto.Name,
                University = dto.University,
                Role = dto.Role,
                RegisteredAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errores = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new ApplicationException("Error al crear usuario: " + errores);
            }

            return new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                University = user.University,
                Role = user.Role,
                RegisteredAt = user.RegisteredAt
            };
        }

        public async Task<UserReadDto?> UpdateAsync(string id, UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            user.Name = dto.Name;
            user.University = dto.University;
            user.Role = dto.Role;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errores = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new ApplicationException("Error al actualizar usuario: " + errores);
            }

            return new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                University = user.University,
                Role = user.Role,
                RegisteredAt = user.RegisteredAt
            };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
