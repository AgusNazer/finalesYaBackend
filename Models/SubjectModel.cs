namespace finalesYaBackend.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Major { get; set; }
        public int? YearTaken { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<Exam> Exams { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}
