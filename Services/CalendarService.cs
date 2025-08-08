
using Microsoft.EntityFrameworkCore;
using finalesYaBackend.DTOs;
using finalesYaBackend.Models;

namespace finalesYaBackend.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly AppDbContext _context;

        public CalendarService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CalendarReadDto>> GetAllAsync()
        {
            var calendars = await _context.Calendars
                .Include(c => c.Usuario)
                .ToListAsync();

            return calendars.Select(calendar => new CalendarReadDto
            {
                Id = calendar.Id,
                Title = calendar.Title,
                StartDate = calendar.StartDate,
                EndDate = calendar.EndDate,
                Description = calendar.Description,
                IsActive = calendar.IsActive,
                CreatedAt = calendar.CreatedAt,
                UsuarioId = calendar.UsuarioId,
                UsuarioName = calendar.Usuario.Name
            });
        }

        public async Task<CalendarReadDto?> GetByIdAsync(string id)
        {
            var calendar = await _context.Calendars
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (calendar == null) return null;

            return new CalendarReadDto
            {
                Id = calendar.Id,
                Title = calendar.Title,
                StartDate = calendar.StartDate,
                EndDate = calendar.EndDate,
                Description = calendar.Description,
                IsActive = calendar.IsActive,
                CreatedAt = calendar.CreatedAt,
                UsuarioId = calendar.UsuarioId,
                UsuarioName = calendar.Usuario.Name
            };
        }

        public async Task<CalendarReadDto> CreateAsync(CalendarCreateDto dto)
        {
            var calendar = new Calendar
            {
                Title = dto.Title,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Description = dto.Description,
                IsActive = dto.IsActive,
                UsuarioId = dto.UsuarioId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Calendars.Add(calendar);
            await _context.SaveChangesAsync();

            var createdCalendar = await _context.Calendars
                .Include(c => c.Usuario)
                .FirstAsync(c => c.Id == calendar.Id);

            return new CalendarReadDto
            {
                Id = createdCalendar.Id,
                Title = createdCalendar.Title,
                StartDate = createdCalendar.StartDate,
                EndDate = createdCalendar.EndDate,
                Description = createdCalendar.Description,
                IsActive = createdCalendar.IsActive,
                CreatedAt = createdCalendar.CreatedAt,
                UsuarioId = createdCalendar.UsuarioId,
                UsuarioName = createdCalendar.Usuario.Name
            };
        }

        public async Task<CalendarReadDto?> UpdateAsync(string id, CalendarCreateDto dto)
        {
            var existing = await _context.Calendars.FindAsync(id);
            if (existing == null) return null;

            existing.Title = dto.Title;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.Description = dto.Description;
            existing.IsActive = dto.IsActive;
            existing.UsuarioId = dto.UsuarioId;

            await _context.SaveChangesAsync();

            var updatedCalendar = await _context.Calendars
                .Include(c => c.Usuario)
                .FirstAsync(c => c.Id == id);

            return new CalendarReadDto
            {
                Id = updatedCalendar.Id,
                Title = updatedCalendar.Title,
                StartDate = updatedCalendar.StartDate,
                EndDate = updatedCalendar.EndDate,
                Description = updatedCalendar.Description,
                IsActive = updatedCalendar.IsActive,
                CreatedAt = updatedCalendar.CreatedAt,
                UsuarioId = updatedCalendar.UsuarioId,
                UsuarioName = updatedCalendar.Usuario.Name
            };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var calendar = await _context.Calendars.FindAsync(id);
            if (calendar == null) return false;

            _context.Calendars.Remove(calendar);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CalendarReadDto>> GetByUserIdAsync(string usuarioId)
        {
            var calendars = await _context.Calendars
                .Include(c => c.Usuario)
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();

            return calendars.Select(calendar => new CalendarReadDto
            {
                Id = calendar.Id,
                Title = calendar.Title,
                StartDate = calendar.StartDate,
                EndDate = calendar.EndDate,
                Description = calendar.Description,
                IsActive = calendar.IsActive,
                CreatedAt = calendar.CreatedAt,
                UsuarioId = calendar.UsuarioId,
                UsuarioName = calendar.Usuario.Name
            });
        }

       public async Task<CalendarViewDto> GetCalendarViewAsync(string usuarioId, int year, int month)
{
    Console.WriteLine($"🔍 GetCalendarViewAsync iniciado - usuarioId: {usuarioId}, {year}/{month}");
    
    var user = await _context.Users.FindAsync(usuarioId);
    if (user == null) throw new ArgumentException("Usuario no encontrado");

    Console.WriteLine($"✅ Usuario encontrado: {user.Name}");

    var startDate = new DateTime(year, month, 1);
    var endDate = startDate.AddMonths(1).AddDays(-1);

    Console.WriteLine($"📅 Rango de fechas: {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}");

    var exams = await GetExamsByDateRangeAsync(usuarioId, startDate, endDate);
    Console.WriteLine($"📝 Exámenes obtenidos de GetExamsByDateRangeAsync: {exams.Count()}");
    
    var statistics = await GetCalendarStatisticsAsync(usuarioId);
    Console.WriteLine($"📊 Estadísticas obtenidas - Total: {statistics.TotalExams}");

    var examsByDate = exams
        .GroupBy(e => e.Date.ToString("yyyy-MM-dd"))
        .ToDictionary(g => g.Key, g => g.ToList());

    Console.WriteLine($"🗓️ ExamsByDate agrupados: {examsByDate.Count} fechas");

    return new CalendarViewDto
    {
        UserId = usuarioId,
        UserName = user.Name,
        Year = year,
        Month = month,
        ExamsByDate = examsByDate,
        Statistics = statistics
    };
}

public async Task<IEnumerable<CalendarExamDto>> GetExamsByDateRangeAsync(string usuarioId, DateTime startDate, DateTime endDate)
{
    Console.WriteLine($"🔍 GetExamsByDateRangeAsync - usuarioId: {usuarioId}");
    Console.WriteLine($"🔍 Rango de fechas: {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}");
    
    // Primero verificar si hay subjects para este usuario
    var userSubjects = await _context.Subjects
        .Where(s => s.UsuarioId == usuarioId)
        .ToListAsync();
    Console.WriteLine($"📚 Subjects del usuario: {userSubjects.Count}");
    foreach(var subject in userSubjects)
    {
        Console.WriteLine($"   - Subject ID: {subject.Id}, Nombre: {subject.Name}");
    }
    
    // Verificar si hay exámenes en general
    var allExamsInRange = await _context.Exams
        .Include(e => e.Subject)
        .Where(e => e.Date >= startDate && e.Date <= endDate)
        .ToListAsync();
    Console.WriteLine($"📝 Todos los exámenes en el rango: {allExamsInRange.Count}");
    foreach(var exam in allExamsInRange)
    {
        Console.WriteLine($"   - Exam ID: {exam.Id}, Subject: {exam.Subject?.Name}, SubjectUsuarioId: {exam.Subject?.UsuarioId}");
    }

    var exams = await _context.Exams
        .Include(e => e.Subject)
        .Where(e => e.Subject.UsuarioId == usuarioId &&
                    e.Date >= startDate &&
                    e.Date <= endDate)
        .OrderBy(e => e.Date)
        .ToListAsync();

    Console.WriteLine($"✅ Exámenes filtrados para usuario: {exams.Count}");

    var today = DateTime.Today;

    return exams.Select(exam => new CalendarExamDto
    {
        Id = exam.Id,
        Type = exam.Type,
        Date = exam.Date,
        Location = exam.Location,
        Passed = exam.Passed,
        SubjectName = exam.Subject.Name,
        SubjectMajor = exam.Subject.Major,
        DaysRemaining = (exam.Date.Date - today).Days,
        IsToday = exam.Date.Date == today,
        IsOverdue = exam.Date.Date < today && exam.Passed == null
    });
}

public async Task<CalendarStatistics> GetCalendarStatisticsAsync(string usuarioId)
{
    Console.WriteLine($"📊 GetCalendarStatisticsAsync - usuarioId: {usuarioId}");
    
    var today = DateTime.Today;
    var weekFromNow = today.AddDays(7);

    var allExams = await _context.Exams
        .Include(e => e.Subject)
        .Where(e => e.Subject.UsuarioId == usuarioId)
        .ToListAsync();

    Console.WriteLine($"📊 Total exámenes del usuario: {allExams.Count}");

    var upcomingExams = allExams.Where(e => e.Date.Date >= today).ToList();
    var nextExam = upcomingExams.OrderBy(e => e.Date).FirstOrDefault();

    return new CalendarStatistics
    {
        TotalExams = allExams.Count,
        UpcomingExams = upcomingExams.Count,
        PassedExams = allExams.Count(e => e.Passed == true),
        FailedExams = allExams.Count(e => e.Passed == false),
        PendingExams = allExams.Count(e => e.Passed == null),
        OverdueExams = allExams.Count(e => e.Date.Date < today && e.Passed == null),
        ExamsThisWeek = allExams.Count(e => e.Date.Date >= today && e.Date.Date <= weekFromNow),
        NextExamDate = nextExam?.Date
    };
}
        public async Task<object> DebugSubjects(string usuarioId)
        {
            var subjects = await _context.Subjects
                .Where(s => s.UsuarioId == usuarioId)
                .Select(s => new { s.Id, s.Name, s.UsuarioId })
                .ToListAsync();
        
            var allSubjects = await _context.Subjects
                .Select(s => new { s.Id, s.Name, s.UsuarioId })
                .ToListAsync();
    
            return new { 
                buscandoUserId = usuarioId,
                subjectsDelUsuario = subjects,
                todosLosSubjects = allSubjects 
            };
        }
    }
    
}
