using finalesYaBackend.services;
using finalesYaBackend.Models;

namespace finalesYaBackend.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        public List<User> GetAll()
        {
            return _users;
        }

        public User? GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User Create(User user)
        {
            user.Id = _nextId++;
            user.RegisteredAt = DateTime.UtcNow;
            _users.Add(user);
            return user;
        }

        public void Update(int id, User updatedUser)
        {
            var existing = GetById(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"No se encontró un usuario con ID {id}");
            }

            existing.Name = updatedUser.Name;
            existing.Email = updatedUser.Email;
            existing.PasswordHash = updatedUser.PasswordHash;
            existing.University = updatedUser.University;
            existing.Role = updatedUser.Role;
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"No se encontró un usuario con ID {id}");
            }

            _users.Remove(user);
        }
    }
}