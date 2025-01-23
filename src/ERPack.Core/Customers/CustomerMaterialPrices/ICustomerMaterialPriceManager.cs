using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Customers.CustomerMaterialPrices
{
    public interface ICustomerMaterialPriceManager : IDomainService
    {
        Task<CustomerMaterialPrice> GetAsync(long id);
        Task<long> CreateAsync(CustomerMaterialPrice customerMaterialPrice);
        void Cancel(CustomerMaterialPrice customerMaterialPrice);
        Task<List<CustomerMaterialPrice>> GetAllAsync();
        Task<List<CustomerMaterialPrice>> GetAllByCustomerIdAsync(long customerId);
    }
}
