using Abp.Domain.Services;
using System.Threading.Tasks;

namespace ERPack.Customers.CustomerTaxationInfos
{
    public interface ICustomerTaxationManager : IDomainService
    {
        Task<CustomerTaxationInfo> GetAsync(long id);
        Task<long> CreateAsync(CustomerTaxationInfo contact);
        Task<CustomerTaxationInfo> GetDefaultTaxationByCustomerAsync(long customerId, int id = 0);
    }
}
