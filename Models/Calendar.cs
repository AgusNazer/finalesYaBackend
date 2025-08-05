namespace finalesYaBackend.Models;

public class Calendar
{
    public string Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones
    public string UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
        
    // Un calendario puede tener múltiples exámenes asociados
    public List<Exam> Exams { get; set; } = new();
}