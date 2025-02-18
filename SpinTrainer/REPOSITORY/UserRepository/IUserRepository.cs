using ENTITYS;

namespace REPOSITORY.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Valida los datos ingresados en el inicio de sesión
        /// </summary>
        /// <param name="username">Código de usuario ingresado</param>
        /// <param name="password">Clave ingresada</param>
        /// <returns>Devuelve un bool que dice es true si el inicio fue exitoso, un string como mensaje de error en caso de que lo haya y el tipo de usuario</returns>
        Task<(bool, string, int)> AuthenticateUser(string username, string password);

        /// <summary>
        /// Inserta el usuario loggeado a la base de datos local para poderlo usar en modo offline.
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <returns></returns>
        Task<(bool, string)> AddUserLoggedToLocalBD(UserEntity user);

        /// <summary>
        /// Inserta un nuevo usuario en la base de datos
        /// </summary>
        /// <param name="newUser">Entidad del nuevo usuario.</param>
        /// <returns>Devuelve un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns>
        Task<(bool, string)> Add(UserEntity newUser);

        /// <summary>
        /// Actualiza los datos de un usuario.
        /// </summary>
        /// <param name="newUserData">Entidad del usuario con los nuevos datos.</param>
        /// <returns>Devuelve un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns>
        Task<(bool, string)> Update(UserEntity newUserData);

        /// <summary>
        /// Actualiza únicamente la clave del usuario.
        /// </summary>
        /// <param name="username">Código de usuario.</param>
        /// <param name="password">Nueva clave.</param>
        /// <returns>Devuelve un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns>
        Task<(bool, string)> UpdatePassword(string username, string password);

        /// <summary>
        /// Elimina un usuario de la base de datos.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Devuelve un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns>
        Task<(bool,string)> Delete(int id);

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">ID a buscar.</param>
        /// <returns>Devuelve el usuario encontrado, un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns>
        Task<(UserEntity, bool, string)> GetById(int id);

        /// <summary>
        /// Obtiene un usuario por su código.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Devuelve el usuario encontrado, un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns>
        Task<(UserEntity, bool, string)> GetByUserName(string username);

        /// <summary>
        /// Valida si un email existe en la base de datos.
        /// </summary>
        /// <param name="email">Email a buscar.</param>
        /// <returns>Devuelve el código del usuario con el email, un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns>
        Task<(string, bool, string)> ValidateUserEmailforUsernameRecovery(string email);

        /// <summary>
        /// Verifica si la membresía de un usuario aun esta vigente.
        /// </summary>
        /// <param name="id">ID Del usuario</param>
        /// <returns>Devuelve un bool para indicar si la membresía esta o no vigente, un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns>
        Task<(bool, bool, string)> VerifyMembershipValidity(int id);

        /// <summary>
        /// Incrementa 1 mes la membresía de un usuario.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Devuelve un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns>
        Task<(bool, string)> IncrementMembership(int id);

        /// <summary>
        /// Obtener una lista de todos los usuarios.
        /// </summary>
        /// <returns>Devuelve una colección genérica con los usuarios encontrados, un bool para indicar si la operación se culmino con éxito o no, un string como mensaje de error en caso de que lo haya.</returns></returns>
        Task<(ICollection<UserEntity>,bool,string)> GetAll();
        
        /// <summary>
        /// Establece que usuario esta usando actualmente el sistema. (Mover a Services)
        /// </summary>
        /// <param name="currentUser">Usuario</param>
        void SetCurrentUser(UserEntity currentUser);

        /// <summary>
        /// Solicitar la información del usuario que esta usando actualmente el sistema. (Mover a Services)
        /// </summary>
        /// <returns>Entidad del usuario.</returns>
        UserEntity GetCurrentUser();

        /// <summary>
        /// Obtiene todos los entrenadores de la base de datos.
        /// </summary>
        /// <returns>Devuelve una colección genérica con los datos de los entrenadores, un bool que indica si la operacion fue exitosa y un mensaje de error en caso de que haya.</returns>
        Task<(ICollection<UserEntity>, bool, string)> GetAllTrainers();

        /// <summary>
        /// Autentica el codigo del usuario ingresado desde la aplicacion de TV.
        /// </summary>
        /// <param name="userID">ID del usuario.</param>
        /// <param name="PIN">PIN ingresado.</param>
        /// <returns>Devuelve un bool para indicar si se inicio correctamente y un int para manejar los mensajes de error.</returns>
        Task<(bool, int)> AuthenticateUserPIN(int userID, string PIN);
    }
}
