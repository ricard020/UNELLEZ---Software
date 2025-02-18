using ENTITYS;
using REPOSITORY.CompanyDataRepository;

namespace SERVICES.CompanyDataService
{
    public class CompanyDataService : ICompanyDataService
    {
        private readonly ICompanyDataRepository _companyDataRepository;

        public CompanyDataService(ICompanyDataRepository companyDataRepository)
        {
            _companyDataRepository = companyDataRepository;
        }

        public async Task<(CompanyDataEntity, bool, string)> LoadCompanyData()
        {
            return await _companyDataRepository.LoadCompanyDataAsync();
        }

        public async Task<(bool, string)> SaveCompanyDataAsync(CompanyDataEntity newCompanyData)
        {
            return await _companyDataRepository.SaveCompanyDataAsync(newCompanyData);
        }
    }
}
