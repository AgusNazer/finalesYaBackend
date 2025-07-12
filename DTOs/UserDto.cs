namespace finalesYaBackend.DTOs
{
    using finalesYaBackend.Models; // Si vas a usar UserRole enum

    public class UserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? University { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}