using Abp.Authorization;
using ERPack.Currency.Dto;
using ERPack.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Currency
{
    [AbpAuthorize]
    public class CurrencyAppService : ERPackAppServiceBase, ICurrencyAppService
    {
        private readonly CurrencyManager _currencyManager;

        public CurrencyAppService(CurrencyManager currencyManager)
        {
            _currencyManager = currencyManager;
        }

        public async Task<List<CurrencyOutput>> GetAllAsync()
        {
            var currencies = await _currencyManager.GetAllAsync();
            var result = ObjectMapper.Map<List<CurrencyOutput>>(currencies);
            return result;
        }
    }
}
