using finalesYaBackend.DTOs;
using finalesYaBackend.Models;

namespace finalesYaBackend.Services
{
    public interface IExamService
    {
        /// <summary>
        /// Obtiene todos los exámenes registrados con información de la materia asociada.
        /// </summary>
        /// <returns>Lista de todos los exámenes</returns>
        Task<IEnumerable<ExamReadDto>> GetAllAsync();
        
        
        /// <summary>
        /// Obtiene los exámenes registrados con información de la materia asociada por userId.
        /// </summary>
        /// <returns>Lista de los exámenes</returns>
        Task<IEnumerable<ExamReadDto>> GetByUserAsync(string userId);

        /// <summary>
        /// Obtiene un examen específico por su ID.
        /// </summary>
        /// <param name="id">ID del examen</param>
        /// <returns>Examen encontrado o null si no existe</returns>
        Task<ExamReadDto?> GetByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo examen asociado a una materia.
        /// </summary>
        /// <param name="dto">Datos del examen a crear</param>
        /// <returns>Examen creado con información completa</returns>
        Task<ExamReadDto> CreateAsync(ExamCreateDto dto);

        /// <summary>
        /// Actualiza un examen existente.
        /// </summary>
        /// <param name="id">ID del examen a actualizar</param>
        /// <param name="dto">Nuevos datos del examen</param>
        /// <returns>Examen actualizado o null si no existe</returns>
        Task<ExamReadDto?> UpdateAsync(int id, ExamCreateDto dto);

        /// <summary>
        /// Elimina un examen por su ID.
        /// </summary>
        /// <param name="id">ID del examen a eliminar</param>
        /// <returns>True si se eliminó correctamente, false si no existe</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Obtiene todos los exámenes de una materia específica.
        /// </summary>
        /// <param name="subjectId">ID de la materia</param>
        /// <returns>Lista de exámenes de la materia especificada</returns>
        Task<IEnumerable<ExamReadDto>> GetBySubjectIdAsync(int subjectId);
    }
}