using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using REPOSITORY.DBContext;
using System.Data;

namespace REPOSITORY.DatabaseRepository
{
    public class DatabaseRepository : IDatabaseRepositoy
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly SQLiteDBContext _sqliteDBContext;
        private static bool _isOnline = false;

        public DatabaseRepository(ApplicationDBContext dbContext, SQLiteDBContext sqliteDBContext)
        {
            _dbContext = dbContext;
            _sqliteDBContext = sqliteDBContext;
        }

        public async Task<bool> ApplyLocalBDMigrationAsync()
        {
            try
            {
                await _sqliteDBContext.Database.MigrateAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string GetConnectionString()
        {
            return _dbContext.Database.GetConnectionString();
        }

        public async Task<bool> TestDataBaseConnectionAsync()
        {
            try
            {
                await _dbContext.Database.OpenConnectionAsync();
                await _dbContext.Database.CloseConnectionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> TestDataBaseConnectionStringAsync(string connectionString)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);

                await connection.OpenAsync();
                if ((connection.State & ConnectionState.Open) > 0)
                {
                    connection.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool GetIsOnlineValue() => _isOnline;

        public void SetIsOnlineValue(bool isOnly) => _isOnline = isOnly;
    }
}
