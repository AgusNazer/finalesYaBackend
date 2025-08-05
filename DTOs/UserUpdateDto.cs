using System.ComponentModel.DataAnnotations;
using finalesYaBackend.Models;

namespace finalesYaBackend.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? University { get; set; }

        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; } = UserRole.User;
    }
}