namespace finalesYaBackend.DTOs;

public class ExamCreateDto
{
    public string Type { get; set; } = null!;
    public DateTime Date { get; set; }
    public string? Location { get; set; }
    public bool? Passed { get; set; }
    public int SubjectId { get; set; }
}
