using Abp.Domain.Services;
using ERPack.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Customers
{
    public interface ICustomerManager : IDomainService
    {
        Task<Customer> GetAsync(long id);
        Task<Customer> GetByIdAsync(long id);
        Task<long> CreateAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        void Cancel(Customer customer);
        Task<List<Customer>> GetAllAsync();
        Task<List<Customer>> SearchCustomersByNameAsync(string name);
        Task<StateMaster> GetStateAsync(int id);
    }
}
