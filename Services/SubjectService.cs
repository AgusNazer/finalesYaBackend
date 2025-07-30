
using Microsoft.EntityFrameworkCore;
using finalesYaBackend.DTOs;
using finalesYaBackend.Models;

namespace finalesYaBackend.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly AppDbContext _context;

        public SubjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubjectReadDto>> GetAllAsync()
        {
            var subjects = await _context.Subjects.ToListAsync();

            return subjects.Select(subject => new SubjectReadDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Major = subject.Major,
                YearTaken = subject.YearTaken
            });
        }

        public async Task<SubjectReadDto?> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return null;

            return new SubjectReadDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Major = subject.Major,
                YearTaken = subject.YearTaken
            };
        }

        public async Task<SubjectReadDto> CreateAsync(SubjectCreateDto dto)
        {
            var subject = new Subject
            {
                Name = dto.Name,
                Major = dto.Major,
                YearTaken = dto.YearTaken
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();  // ← Guardar en la DB real

            return new SubjectReadDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Major = subject.Major,
                YearTaken = subject.YearTaken
            };
        }

        public async Task<SubjectReadDto?> UpdateAsync(int id, SubjectCreateDto dto)
        {
            var existing = await _context.Subjects.FindAsync(id);
            if (existing == null) return null;

            existing.Name = dto.Name;
            existing.Major = dto.Major;
            existing.YearTaken = dto.YearTaken;

            await _context.SaveChangesAsync();  // ← Guardar cambios

            return new SubjectReadDto
            {
                Id = existing.Id,
                Name = existing.Name,
                Major = existing.Major,
                YearTaken = existing.YearTaken
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return false;

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();  // ← Guardar cambios

            return true;
        }
    }
}