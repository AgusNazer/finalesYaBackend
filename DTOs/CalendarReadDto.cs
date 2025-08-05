namespace finalesYaBackend.DTOs
{
    public class CalendarReadDto
    {
        public string Id { get; set; }              // corregido: era int, pero en tu modelo es string
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UsuarioId { get; set; }       // corregido: era int y se llamaba UserId
        public string UsuarioName { get; set; } = null!;  // corregido: antes UserName

        // Exámenes agrupados por fecha para vista de calendario (opcional si se usa en la vista extendida)
        public Dictionary<string, List<CalendarExamDto>> ExamsByDate { get; set; } = new();
    }
}