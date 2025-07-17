using System.ComponentModel.DataAnnotations;
using finalesYaBackend.Models;

namespace finalesYaBackend.DTOs
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = null!; // <- NO PasswordHash

        public string? University { get; set; }

        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; } = UserRole.User;
    }
}