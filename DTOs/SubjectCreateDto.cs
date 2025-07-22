namespace finalesYaBackend.DTOs;

public class SubjectCreateDto
{
    public string Name { get; set; } = null!;
    public string? Major { get; set; }
    public int? YearTaken { get; set; }
}
