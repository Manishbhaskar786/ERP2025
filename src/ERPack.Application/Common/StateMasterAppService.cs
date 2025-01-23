using Abp.Domain.Repositories;
using ERPack.Common.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ERPack.Common
{
    public class StateMasterAppService : ERPackAppServiceBase, IStateMasterAppService
    {
        private readonly IRepository<StateMaster, int> _StateMasterRepository;

        public StateMasterAppService(IRepository<StateMaster, int> stateMasterRepository)
        {
            _StateMasterRepository = stateMasterRepository;
        }
        public async Task<List<StateMasterDto>> GetStatesAsync(int CountryId)
        {
            var States = new List<StateMaster>();
            var query = CreateFilteredQuery(CountryId);
            query = ApplySorting(query);

            States = query.ToList();
            var result = ObjectMapper.Map<List<StateMasterDto>>(States);
            return result;
        }

        protected IQueryable<StateMaster> CreateFilteredQuery(int CountryId)
        {
            return _StateMasterRepository.GetAll().IgnoreQueryFilters()
                .WhereIf(CountryId != 0, x =>  x.IsDeleted == false).AsQueryable();
        }
        protected IQueryable<StateMaster> ApplySorting(IQueryable<StateMaster> query)
        {
            return query.OrderBy(r => r.StateName);
        }
    }
}
