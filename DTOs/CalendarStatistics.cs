// DTOs/CalendarStatistics.cs - Estadísticas del calendario
namespace finalesYaBackend.DTOs
{
    public class CalendarStatistics
    {
        public int TotalExams { get; set; }
        public int UpcomingExams { get; set; }
        public int PassedExams { get; set; }
        public int FailedExams { get; set; }
        public int PendingExams { get; set; }
        public int OverdueExams { get; set; }
        public int ExamsThisWeek { get; set; }
        public DateTime? NextExamDate { get; set; }
    }
}