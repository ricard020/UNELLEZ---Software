using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using REPOSITORY.DBContext;
using System.Data;

namespace REPOSITORY.TempDatabaseRepository
{
    public class TempDatabaseRepository : ITempDatabaseRepository
    {
        private readonly SQLiteDBContext _dbContext;

        public TempDatabaseRepository(SQLiteDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ApplyMigrationAsync()
        {
            try
            {
                await _dbContext.Database.MigrateAsync();
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
    }
}
