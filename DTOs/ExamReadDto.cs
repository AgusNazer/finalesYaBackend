namespace finalesYaBackend.DTOs;

public class ExamReadDto
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public DateTime Date { get; set; }
    public string? Location { get; set; }
    public bool? Passed { get; set; }
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = null!; // Para mostrar el nombre de la materia
}
