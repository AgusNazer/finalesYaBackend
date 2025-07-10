namespace finalesYaBackend.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
