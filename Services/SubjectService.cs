using finalesYaBackend.DTOs;
using finalesYaBackend.Models;

namespace finalesYaBackend.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly List<Subject> _subjects = new();
        private int _nextId = 1;

        public async Task<IEnumerable<SubjectReadDto>> GetAllAsync()
        {
            await Task.Delay(1); // Simula operación asíncrona

            return _subjects.Select(subject => new SubjectReadDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Major = subject.Major,
                YearTaken = subject.YearTaken
            });
        }


        public async Task<SubjectReadDto?> GetByIdAsync(int id)
        {
            await Task.Delay(1);

            var subject = _subjects.FirstOrDefault(s => s.Id == id);
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
            await Task.Delay(1);
    
            var subject = new Subject
            {
                Id = _nextId++,
                Name = dto.Name,
                Major = dto.Major,
                YearTaken = dto.YearTaken
            };

            _subjects.Add(subject);

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
            await Task.Delay(1);

            var existing = _subjects.FirstOrDefault(s => s.Id == id);
            if (existing == null) return null;

            existing.Name = dto.Name;
            existing.Major = dto.Major;
            existing.YearTaken = dto.YearTaken;

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
            await Task.Delay(1);

            var subject = _subjects.FirstOrDefault(s => s.Id == id);
            if (subject == null) return false;

            _subjects.Remove(subject);
            return true;
        }
    }
}