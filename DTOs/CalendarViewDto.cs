namespace finalesYaBackend.DTOs
{
    public class CalendarViewDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int Year { get; set; }
        public int Month { get; set; }
        public Dictionary<string, List<CalendarExamDto>> ExamsByDate { get; set; } = new();
        public CalendarStatistics Statistics { get; set; } = new();
    }
}