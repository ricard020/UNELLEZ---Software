using ENTITYS;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using REPOSITORY.DBContext;
using System.Data;

namespace REPOSITORY.CompanyDataRepository
{
    public class CompanyDataRepository : ICompanyDataRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public CompanyDataRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<(CompanyDataEntity, bool, string)> LoadCompanyDataAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();

                try
                {
                    var companyDataEntity = await dbContext.CompanyData.FirstOrDefaultAsync();

                    return (companyDataEntity, true, "");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return (null, false, ex.Message);
                }
            }
        }

        public async Task<(bool,string)> SaveCompanyDataAsync(CompanyDataEntity newCompanyData)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var dbContext = _serviceProvider.GetService<ApplicationDBContext>())
                {
                    using (var transaction = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var companyData = await dbContext.CompanyData.FirstOrDefaultAsync();

                            if (companyData != null)
                            {
                                dbContext.CompanyData.Remove(companyData);
                            }

                            await dbContext.CompanyData.AddAsync(newCompanyData);

                            await dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();

                            return (true, "");
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return (false, ex.Message);
                        }
                    }
                }
            }
        }
    }
}
