
using REPOSITORY.DatabaseRepository;
using UTILITIES.CryptographyDataUtility;

namespace SERVICES.DatabaseServices
{
    public class DatabaseServices : IDatabaseServices
    {
        private readonly ICryptographyDataUtility _cryptographyDataUtility;
        private readonly IDatabaseRepositoy _databaseRepository;

        public DatabaseServices(ICryptographyDataUtility cryptographyDataUtility, IDatabaseRepositoy databaseRepository)
        {
            _cryptographyDataUtility = cryptographyDataUtility;
            _databaseRepository = databaseRepository;
        }

        public async Task<bool> ApplyLocalBDMigrationAsync()
        {
            return await _databaseRepository.ApplyLocalBDMigrationAsync();
        }

        public string GetConnectionString()
        {
            return _databaseRepository.GetConnectionString();
        }

        public void SaveDatabaseConnection(string connectionString)
        {
            string executablePath = AppDomain.CurrentDomain.BaseDirectory; // Obtiene la ruta del ejecutable
            string programPath = Path.GetDirectoryName(executablePath);
            string fileName = "Application.cfg"; // Nombre del archivo que deseas verificar o crear            
            string parentPath = Directory.GetParent(programPath).FullName;
            string filePath = Path.Combine(parentPath, fileName);

            string connectionStringEncriptada = _cryptographyDataUtility.Encrypt(connectionString);

            File.WriteAllText(filePath, connectionStringEncriptada);
        }

        public async Task<bool> TestDataBaseConnectionAsync()
        {
            return await _databaseRepository.TestDataBaseConnectionAsync();
        }

        public async Task<bool> TestDataBaseConnectionStringAsync(string connectionString)
        {
            return await _databaseRepository.TestDataBaseConnectionStringAsync(connectionString);
        }


        public bool GetIsOnlineValue() => _databaseRepository.GetIsOnlineValue();

        public void SetIsOnlineValue(bool isOnline) => _databaseRepository.SetIsOnlineValue(isOnline);
    }
}
