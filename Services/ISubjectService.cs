using finalesYaBackend.DTOs;
using finalesYaBackend.Models;

namespace finalesYaBackend.Services;

public interface ISubjectService
{
    /// <summary>
    /// Devuelve todos los subjects de forma asíncrona.
    /// </summary>
    Task<IEnumerable<SubjectReadDto>> GetAllAsync();
    
    /// <summary>
    /// Devuelve todos los subjects por id del user de forma asíncrona.
    /// </summary>
    Task<IEnumerable<SubjectReadDto>> GetByUserAsync(string userId);

    /// <summary>
    /// Devuelve un subject por ID de forma asíncrona.
    /// </summary>
    /// <param name="id">ID del subject</param>
    /// <returns>Subject encontrado o null si no existe</returns>
    Task<SubjectReadDto?> GetByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo subject de forma asíncrona.
    /// </summary>
    /// <param name="dto">DTO con los datos del nuevo subject</param>
    /// <returns>Subject creado</returns>
    Task<SubjectReadDto> CreateAsync(SubjectCreateDto dto);

    /// <summary>
    /// Actualiza un subject existente de forma asíncrona.
    /// </summary>
    /// <param name="id">ID del subject a actualizar</param>
    /// <param name="subject">Datos actualizados del subject</param>
    /// <returns>Subject actualizado o null si no existe</returns>
    Task<SubjectReadDto?> UpdateAsync(int id, SubjectCreateDto dto);

    /// <summary>
    /// Elimina un subject por ID de forma asíncrona.
    /// </summary>
    /// <param name="id">ID del subject a eliminar</param>
    /// <returns>True si se eliminó correctamente, false si no existe</returns>
    Task<bool> DeleteAsync(int id);
}