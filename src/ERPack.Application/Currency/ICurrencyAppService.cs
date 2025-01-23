using Abp.Application.Services;
using ERPack.Currency.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Customers
{
    public interface ICurrencyAppService : IApplicationService
    {
        Task<List<CurrencyOutput>> GetAllAsync();
    }
}
