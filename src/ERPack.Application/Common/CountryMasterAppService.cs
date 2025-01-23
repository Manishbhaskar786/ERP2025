using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERPack.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Common
{
    public class CountryMasterAppService : ERPackAppServiceBase, ICountryMasterAppService
    {
        private readonly IRepository<CountryMaster, int> _countrymasterRepository;

        public CountryMasterAppService(IRepository<CountryMaster, int> countrymasterRepository)
        {
            _countrymasterRepository = countrymasterRepository;
        }

        public async Task<List<CountryMasterDto>> GetCountriesAsync(int CountryId)
        {
            var Countries = new List<CountryMaster>();
            var query = CreateFilteredQuery(CountryId);
            query = ApplySorting(query);

            Countries = query.ToList();
            var result = ObjectMapper.Map<List<CountryMasterDto>>(Countries);
            return result;
        }

        protected IQueryable<CountryMaster> CreateFilteredQuery(int CountryId)
        {
            return _countrymasterRepository.GetAll().IgnoreQueryFilters()
                .WhereIf(CountryId != 0, x => x.IsDeleted == false).AsQueryable();
        }
        protected IQueryable<CountryMaster> ApplySorting(IQueryable<CountryMaster> query)
        {
            return query.OrderBy(r => r.CountryName);
        }
    }
}
