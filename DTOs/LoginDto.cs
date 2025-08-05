namespace finalesYaBackend.DTOs;

public class LoginDto
{
    // Para login
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

// Para registro simple
    public class RegisterDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? University { get; set; }
    }
}