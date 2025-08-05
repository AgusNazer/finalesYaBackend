using finalesYaBackend.Models;

namespace finalesYaBackend.DTOs
{
   public class UserReadDto
   {
       public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? University { get; set; }
        public UserRole Role { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}

// dto sin password, passwordHash, Subjects, Commnets