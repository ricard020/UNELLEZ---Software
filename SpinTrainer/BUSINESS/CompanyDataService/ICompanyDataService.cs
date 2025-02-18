using ENTITYS;

namespace SERVICES.CompanyDataService
{
    public interface ICompanyDataService
    {
        /// <summary>
        /// Actualizar los datos de le empresa en la base de datos.
        /// </summary>
        /// <param name="companyData">Datos de la empresa.</param>
        /// <returns>Booleano para indicar si la operación fue exitosa y un string con un mensaje de error en caso de que lo de.</returns>
        Task<(bool, string)> SaveCompanyDataAsync(CompanyDataEntity newCompanyData);

        /// <summary>
        /// Carga los datos de la empresa.
        /// </summary>
        /// <returns>Devuelve un modelo de datos con la información, booleano para indicar si la operación fue exitosa y un string con un mensaje de error en caso de que lo de.</returns>
        Task<(CompanyDataEntity, bool, string)> LoadCompanyData();
    }
}
