namespace finalesYaBackend.DTOs;

public class SubjectReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Major { get; set; }
    public int? YearTaken { get; set; }

    // public List<ExamDto>? Exams { get; set; } // Opcional
    // public List<CommentDto>? Comments { get; set; } // Opcional
}
