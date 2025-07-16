namespace finalesYaBackend.Models

{
    public enum UserRole
    {
        User,
        
        
        SuperUser
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? University { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public List<Subject> Subjects { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}
