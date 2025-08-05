using finalesYaBackend.DTOs;
using finalesYaBackend.Models;

namespace finalesYaBackend.Services
{
    public interface ICalendarService
    {
        /// <summary>
        /// Obtiene todos los calendarios registrados.
        /// </summary>
        /// <returns>Lista de todos los calendarios</returns>
        Task<IEnumerable<CalendarReadDto>> GetAllAsync();

        /// <summary>
        /// Obtiene un calendario específico por su ID.
        /// </summary>
        /// <param name="id">ID del calendario</param>
        /// <returns>Calendario encontrado o null si no existe</returns>
        Task<CalendarReadDto?> GetByIdAsync(string id);

        /// <summary>
        /// Crea un nuevo calendario para un usuario.
        /// </summary>
        /// <param name="dto">Datos del calendario a crear</param>
        /// <returns>Calendario creado con información completa</returns>
        Task<CalendarReadDto> CreateAsync(CalendarCreateDto dto);

        /// <summary>
        /// Actualiza un calendario existente.
        /// </summary>
        /// <param name="id">ID del calendario a actualizar</param>
        /// <param name="dto">Nuevos datos del calendario</param>
        /// <returns>Calendario actualizado o null si no existe</returns>
        Task<CalendarReadDto?> UpdateAsync(string id, CalendarCreateDto dto);

        /// <summary>
        /// Elimina un calendario por su ID.
        /// </summary>
        /// <param name="id">ID del calendario a eliminar</param>
        /// <returns>True si se eliminó correctamente, false si no existe</returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Obtiene todos los calendarios de un usuario específico.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de calendarios del usuario</returns>
        Task<IEnumerable<CalendarReadDto>> GetByUserIdAsync(string userId);

        /// <summary>
        /// Obtiene la vista del calendario de un usuario por mes y año.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="year">Año</param>
        /// <param name="month">Mes (1-12)</param>
        /// <returns>Vista del calendario con exámenes agrupados por fecha</returns>
        Task<CalendarViewDto> GetCalendarViewAsync(string userId, int year, int month);

        /// <summary>
        /// Obtiene todos los exámenes de un usuario en un rango de fechas.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha de fin</param>
        /// <returns>Lista de exámenes en el rango especificado</returns>
        Task<IEnumerable<CalendarExamDto>> GetExamsByDateRangeAsync(string userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene las estadísticas del calendario de un usuario.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Estadísticas del calendario</returns>
        Task<CalendarStatistics> GetCalendarStatisticsAsync(string userId);
    }
}