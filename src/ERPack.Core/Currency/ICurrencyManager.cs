using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Currency
{
    public interface ICurrencyManager : IDomainService
    {
        Task<List<CurrencyMaster>> GetAllAsync();
        Task<CurrencyMaster> GetCurrencyByIdAsync(int id);
    }
}
