namespace REPOSITORY.TempDatabaseRepository
{
    internal interface ITempDatabaseRepository
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
        Task<bool> ApplyMigrationAsync();
    }
}
