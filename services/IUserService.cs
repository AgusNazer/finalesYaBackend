namespace finalesYaBackend.services
{
    using finalesYaBackend.Models;

    public interface IUserService
    {
        /// <summary>
        /// Devuelve todos los usuarios.
        /// </summary>
        List<User> GetAll();

        /// <summary>
        /// Devuelve un usuario por ID.
        /// </summary>
        User? GetById(int id);

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        User Create(User user);

        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        void Update(int id, User user);

        /// <summary>
        /// Elimina un usuario por ID.
        /// </summary>
        void Delete(int id);
    }
}