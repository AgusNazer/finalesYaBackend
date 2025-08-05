
namespace finalesYaBackend.DTOs
{
    public class CalendarCreateDto
    {
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string UsuarioId { get; set; }
    }
}