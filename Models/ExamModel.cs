namespace finalesYaBackend.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!; // Midterm or Final
        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public bool? Passed { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
    }
}
