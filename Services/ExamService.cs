using Microsoft.EntityFrameworkCore;
using finalesYaBackend.DTOs;
using finalesYaBackend.Models;

namespace finalesYaBackend.Services
{
    public class ExamService : IExamService
    {
        private readonly AppDbContext _context;

        public ExamService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExamReadDto>> GetAllAsync()
        {
            var exams = await _context.Exams
                .Include(e => e.Subject)
                .ToListAsync();

            return exams.Select(exam => new ExamReadDto
            {
                Id = exam.Id,
                Type = exam.Type,
                Date = exam.Date,
                Location = exam.Location,
                Passed = exam.Passed,
                SubjectId = exam.SubjectId,
                SubjectName = exam.Subject.Name
            });
        }

        public async Task<ExamReadDto?> GetByIdAsync(int id)
        {
            var exam = await _context.Exams
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exam == null) return null;

            return new ExamReadDto
            {
                Id = exam.Id,
                Type = exam.Type,
                Date = exam.Date,
                Location = exam.Location,
                Passed = exam.Passed,
                SubjectId = exam.SubjectId,
                SubjectName = exam.Subject.Name
            };
        }

        public async Task<ExamReadDto> CreateAsync(ExamCreateDto dto)
        {
            var exam = new Exam
            {
                Type = dto.Type,
                Date = dto.Date,
                Location = dto.Location,
                Passed = dto.Passed,
                SubjectId = dto.SubjectId
            };

            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            // Obtener el examen con la información del Subject
            var createdExam = await _context.Exams
                .Include(e => e.Subject)
                .FirstAsync(e => e.Id == exam.Id);

            return new ExamReadDto
            {
                Id = createdExam.Id,
                Type = createdExam.Type,
                Date = createdExam.Date,
                Location = createdExam.Location,
                Passed = createdExam.Passed,
                SubjectId = createdExam.SubjectId,
                SubjectName = createdExam.Subject.Name
            };
        }

        public async Task<ExamReadDto?> UpdateAsync(int id, ExamCreateDto dto)
        {
            var existing = await _context.Exams.FindAsync(id);
            if (existing == null) return null;

            existing.Type = dto.Type;
            existing.Date = dto.Date;
            existing.Location = dto.Location;
            existing.Passed = dto.Passed;
            existing.SubjectId = dto.SubjectId;

            await _context.SaveChangesAsync();

            // Obtener el examen actualizado con la información del Subject
            var updatedExam = await _context.Exams
                .Include(e => e.Subject)
                .FirstAsync(e => e.Id == id);

            return new ExamReadDto
            {
                Id = updatedExam.Id,
                Type = updatedExam.Type,
                Date = updatedExam.Date,
                Location = updatedExam.Location,
                Passed = updatedExam.Passed,
                SubjectId = updatedExam.SubjectId,
                SubjectName = updatedExam.Subject.Name
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null) return false;

            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ExamReadDto>> GetBySubjectIdAsync(int subjectId)
        {
            var exams = await _context.Exams
                .Include(e => e.Subject)
                .Where(e => e.SubjectId == subjectId)
                .ToListAsync();

            return exams.Select(exam => new ExamReadDto
            {
                Id = exam.Id,
                Type = exam.Type,
                Date = exam.Date,
                Location = exam.Location,
                Passed = exam.Passed,
                SubjectId = exam.SubjectId,
                SubjectName = exam.Subject.Name
            });
        }
    }
}