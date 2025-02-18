using ENTITYS;
using System.Collections.ObjectModel;

namespace SERVICES.SessionServices
{
    public interface ISessionServices
    {
        /// <summary>
        /// Elimina una sesión de la base de datos.
        /// </summary>
        /// <param name="id">Id de la sesión a eliminar</param>
        /// <returns>Devuelve un bool para indicar si la operación fue exitosa y un mensaje de error en caso de que lo de.</returns>
        Task<(bool, string)> Delete(int id);

        /// <summary>
        /// Elimina una sesión de la base de datos del telefono.
        /// </summary>
        /// <param name="id">Id de la sesión a eliminar</param>
        /// <returns>Devuelve un bool para indicar si la operación fue exitosa y un mensaje de error en caso de que lo de.</returns>
        Task<(bool, string)> DeleteSessionInLocalDb(int id);

        /// <summary>
        /// Actualiza los datos de una sesión en la base de datos.
        /// </summary>
        /// <param name="session">Nuevos datos de la sesión.</param>
        /// <returns>Devuelve un bool para indicar si la operación fue exitosa y un mensaje de error en caso de que lo de.</returns>
        Task<(bool, string)> Update(SessionEntity session);

        /// <summary>
        /// Agregar nueva sesión a la base de datos.
        /// </summary>
        /// <param name="session">Sesión.</param>
        /// <returns>Devuelve un bool para indicar si la operación fue exitosa y un mensaje de error en caso de que lo de.</returns>
        Task<(bool, string)> Add(SessionEntity session);

        /// <summary>
        /// Obtiene una sesión por su ID.
        /// </summary>
        /// <param name="id">ID de la sesión.</param>
        /// <returns>Devuelve los datos de la sesión, un bool para indicar si la operación fue exitosa y un mensaje de error en caso de que lo de.</returns>
        Task<(SessionEntity, bool, string)> GetByID(int id);

        /// <summary>
        /// Obtiene todas las sesiones con una fecha e entrenador especifico y permite buscar por descripción opcionalmente.
        /// </summary>
        /// <param name="dateI">Fecha de inicio de la sesión.</param>
        /// <param name="idEntrenador">ID del entrenador a cargo.</param>
        /// <param name="descrip">Parámetro opcional para buscar por descripción.</param>
        /// <returns>Devuelve una ObservableCollection de las sesiones encontradas, un int con los registros encontrados, un bool para indicar si la operación fue exitosa y un mensaje de error en caso de que lo de.</returns>
        Task<(ObservableCollection<SessionEntity>, int, bool, string)> GetAllByFilters(DateTime? dateI, int idEntrenador, string? descrip, int skip);

    }
}
