using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERPack.Currency
{
    public class CurrencyManager : ICurrencyManager
    {
        private readonly IRepository<CurrencyMaster, int> _currencyRepository;

        public CurrencyManager(IRepository<CurrencyMaster, int> currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<List<CurrencyMaster>> GetAllAsync()
        {
            var currencies = await _currencyRepository.GetAll().IgnoreQueryFilters().Where(x => !x.IsDeleted).ToListAsync();
            if (currencies == null)
            {
                throw new UserFriendlyException("No currencies found, please contact admin!");
            }
            return currencies;
        }

        public async Task<CurrencyMaster> GetCurrencyByIdAsync(int id)
        {
            var currency = await _currencyRepository.GetAll().IgnoreQueryFilters().Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (currency == null)
            {
                throw new UserFriendlyException("No currency found, please contact admin!");
            }
            return currency;
        }
    }
}
