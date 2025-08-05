using finalesYaBackend.DTOs;

namespace finalesYaBackend.Services // PascalCase para carpetas
{
    using finalesYaBackend.Models;

    public interface IUserService
    {
        /// <summary>
        /// Devuelve todos los usuarios de forma asíncrona.
        /// </summary>
        Task<IEnumerable<UserReadDto>> GetAllAsync();

        /// <summary>
        /// Devuelve un usuario por ID de forma asíncrona.
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Usuario encontrado o null si no existe</returns>
        Task<UserReadDto?> GetByIdAsync(string id);

        /// <summary>
        /// Crea un nuevo usuario de forma asíncrona.
        /// </summary>
        /// <param name="user">Usuario a crear</param>
        /// <returns>Usuario creado</returns>
        Task<UserReadDto> CreateAsync(UserCreateDto dto);

        /// <summary>
        /// Actualiza un usuario existente de forma asíncrona.
        /// </summary>
        /// <param name="id">ID del usuario a actualizar</param>
        /// <param name="user">Datos del usuario actualizado</param>
        /// <returns>Usuario actualizado o null si no existe</returns>
        Task<UserReadDto?> UpdateAsync(string id, UserUpdateDto dto);

        /// <summary>
        /// Elimina un usuario por ID de forma asíncrona.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente, false si no existe</returns>
        Task<bool> DeleteAsync(string id);
    }
}