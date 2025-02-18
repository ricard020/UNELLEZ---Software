namespace REPOSITORY.DatabaseRepository
{
    public interface IDatabaseRepositoy
    {
        /// <summary>
        /// Probar conexión a la base de datos.
        /// </summary>
        /// <returns>Devuelve un booleano para indicar si la conexión fue exitosa o no</returns>
        Task<bool> TestDataBaseConnectionAsync();

        /// <summary>
        /// Probar una cadena de conexión a la base de datos.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión.</param>
        /// <returns>Devuelve un booleano para indicar si la conexión fue exitosa o no</returns>
        Task<bool> TestDataBaseConnectionStringAsync(string connectionString);

        /// <summary>
        /// Aplicar migraciones a la base de datos.
        /// </summary>
        /// <returns>Devuelve un booleano para indicar si la conexión fue exitosa o no</returns>
        Task<bool> ApplyLocalBDMigrationAsync();

        /// <summary>
        /// Obtener cadena de conexión configurada actualmente en la aplicación (sirve principalmente para pasarle la cadena de conexión a los reportes)
        /// </summary>
        /// <returns>Devuelve la cadena de conexión</returns>
        string GetConnectionString();

        /// <summary>
        /// Pregunta si la base de datos esta en línea.
        /// </summary>
        /// <returns>Devuelve un booleano para indicar si esta en línea o no.</returns>
        public bool GetIsOnlineValue();

        /// <summary>
        /// Establece el estado de conexión con la base de datos
        /// </summary>
        /// <param name="isOnline">Valor para indicar si esta en línea o no.</param>
        public void SetIsOnlineValue(bool isOnline);
    }
}