using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Customers.CustomerTaxationInfos
{
    public class CustomerTaxationManager : ICustomerTaxationManager
    {
        private readonly IRepository<CustomerTaxationInfo, int> _repository;

        public CustomerTaxationManager(
            IRepository<CustomerTaxationInfo, int> repository)
        {
            _repository = repository;
        }

        public async Task<long> CreateAsync(CustomerTaxationInfo taxation)
        {
            return await _repository.InsertAndGetIdAsync(taxation);
        }

        public async Task<CustomerTaxationInfo> UpdateAsync(CustomerTaxationInfo taxation)
        {
            return await _repository.UpdateAsync(taxation);
        }

        public async Task<CustomerTaxationInfo> GetAsync(long id)
        {
            var taxation = await _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (taxation == null)
            {
                throw new UserFriendlyException("Could not found the taxation, maybe it's deleted!");
            }
            return taxation;

        }

        public async Task<CustomerTaxationInfo> GetDefaultTaxationByCustomerAsync(long customerId, int id = 0)
        {
            var taxation = await _repository.GetAll().Include(x => x.State).Where(x => x.CustomerId == customerId && x.IsDefault && (id == 0 || x.Id != id)).FirstOrDefaultAsync();
            return taxation;
        }
    }
}
