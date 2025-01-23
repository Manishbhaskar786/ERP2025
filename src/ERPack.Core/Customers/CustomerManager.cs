using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Customers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly IRepository<Customer, long> _customerRepository;
        private readonly IRepository<StateMaster, int> _stateRepository;

        public CustomerManager(
            IRepository<Customer, long> customerRepository, IRepository<StateMaster, int> stateRepository)
        {
            _customerRepository = customerRepository;
            _stateRepository = stateRepository;
        }

        public async Task<long> CreateAsync(Customer customer)
        {
            return await _customerRepository.InsertAndGetIdAsync(customer);
        }
        public async Task<Customer> UpdateAsync(Customer customer)
        {
            return await _customerRepository.UpdateAsync(customer);
        }

        public async Task<Customer> GetAsync(long id)
        {
            var cusotmer = await _customerRepository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (cusotmer == null)
            {
                throw new UserFriendlyException("Could not found the cusotmer, maybe it's deleted!");
            }
            return cusotmer;

        }

        public async Task<Customer> GetByIdAsync(long id)
        {
            var cusotmer = await _customerRepository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();
            return cusotmer;

        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAll().ToListAsync();

            if (customers == null)
            {
                throw new UserFriendlyException("No customer found, please contact admin!");
            }
            return customers;

        }

        public async Task<List<Customer>> SearchCustomersByNameAsync(string name)
        {
            var customers = await _customerRepository.GetAll().
                 Where(r =>r.Name.Contains(name))
                .ToListAsync();

            if (customers == null)
            {
                throw new UserFriendlyException("No customer found, please contact admin!");
            }
            return customers;

        }

        public async Task<StateMaster> GetStateAsync(int id)
        {

            var st = await _stateRepository.GetAsync(id);

            var state = await _stateRepository.GetAll().IgnoreQueryFilters().
                 Where(r => r.Id == id && !r.IsDeleted)
                .FirstOrDefaultAsync();

            if (state == null)
            {
                throw new UserFriendlyException("No state found, please contact admin!");
            }
            return state;
        }

        public void Cancel(Customer customer)
        {
            _customerRepository.Delete(customer);
        }
    }
}
