namespace finalesYaBackend.DTOs
{
    public class CalendarExamDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public bool? Passed { get; set; }
        public string SubjectName { get; set; } = null!;
        public string SubjectMajor { get; set; } = null!;
        public int DaysRemaining { get; set; }
        public bool IsToday { get; set; }
        public bool IsOverdue { get; set; }
    }
}