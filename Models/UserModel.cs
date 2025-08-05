using Microsoft.AspNetCore.Identity;

namespace finalesYaBackend.Models
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class Usuario : IdentityUser
    {
        // Identity ya incluye: Id, Email, UserName, PasswordHash, etc.
        public string Name { get; set; } = null!;
        public string? University { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        // Tus relaciones existentes
        public List<Subject> Subjects { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}






////////// CLASE SIN IDENTITY USER
// namespace finalesYaBackend.Models
//
// {
//     public enum UserRole
//     {
//         User,
//         
//         
//         SuperUser
//     }
//
//     public class User
//     {
//         public int Id { get; set; }
//         public string Name { get; set; } = null!;
//         public string Email { get; set; } = null!;
//         public string PasswordHash { get; set; } = null!;
//         public string? University { get; set; }
//         public UserRole Role { get; set; } = UserRole.User;
//         public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
//
//         public List<Subject> Subjects { get; set; } = new();
//         public List<Comment> Comments { get; set; } = new();
//     }
// }
