namespace finalesYaBackend.DTOs
{
    public class CalendarReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        
        // Exámenes agrupados por fecha para vista de calendario
        public Dictionary<string, List<CalendarExamDto>> ExamsByDate { get; set; } = new();
    }
}