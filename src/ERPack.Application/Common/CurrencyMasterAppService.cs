using Abp.Domain.Repositories;
using ERPack.Common.Dto;
using ERPack.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Collections.Extensions;

namespace ERPack.Common
{
    public class CurrencyMasterAppService : ERPackAppServiceBase, ICurrencyMasterAppService
    {
        private readonly IRepository<CurrencyMaster, int> _currencymasterRepository;

        public CurrencyMasterAppService(IRepository<CurrencyMaster, int> currencymasterRepository)
        {
            _currencymasterRepository = currencymasterRepository;
        }

        public async Task<List<CurrencyMasterDto>> GetCurrenciesAsync(int currencyId = 0)
        {
            var Currencies = new List<CurrencyMaster>();
            var query = CreateFilteredQuery(currencyId);
            query = ApplySorting(query);

            Currencies = query.ToList();
            var result = ObjectMapper.Map<List<CurrencyMasterDto>>(Currencies);
            return result;
        }
        protected IQueryable<CurrencyMaster> CreateFilteredQuery(int Id)
        {
            return _currencymasterRepository.GetAll()
                .WhereIf(Id != 0, x => x.IsDeleted == false).AsQueryable();
        }
        protected IQueryable<CurrencyMaster> ApplySorting(IQueryable<CurrencyMaster> query)
        {
            return query.OrderBy(r => r.Name);
        }
    }
}
