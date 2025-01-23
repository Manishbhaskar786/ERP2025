using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Departments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Customers.CustomerMaterialPrices
{
    public class CustomerMaterialPriceManager : ICustomerMaterialPriceManager
    {
        private readonly IRepository<CustomerMaterialPrice, long> _repository;

        public CustomerMaterialPriceManager(
            IRepository<CustomerMaterialPrice, long> repository)
        {
            _repository = repository;
        }

        public async Task<long> CreateAsync(CustomerMaterialPrice customerMaterialPrice)
        {
            return await _repository.InsertAndGetIdAsync(customerMaterialPrice);

        }

        public async Task<CustomerMaterialPrice> GetAsync(long id)
        {
            var customerMaterialPrice = await _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (customerMaterialPrice == null)
            {
                throw new UserFriendlyException("Could not found the CustomerMaterialPrice, maybe it's deleted!");
            }
            return customerMaterialPrice;

        }

        public async Task<List<CustomerMaterialPrice>> GetAllAsync()
        {
            var customerMaterialPrices = await _repository.GetAll().ToListAsync();

            if (customerMaterialPrices == null)
            {
                throw new UserFriendlyException("No CustomerMaterialPrice found, please contact admin!");
            }
            return customerMaterialPrices;
        }

        public async Task<List<CustomerMaterialPrice>> GetAllByCustomerIdAsync(long customerId)
        {
            var customerMaterialPrices = await _repository.GetAll().Where(x=> x.CustomerId == customerId).ToListAsync();

            if (customerMaterialPrices == null)
            {
                throw new UserFriendlyException("No CustomerMaterialPrice found, please contact admin!");
            }
            return customerMaterialPrices;
        }

        public void Cancel(CustomerMaterialPrice customerMaterialPrice)
        {
            _repository.Delete(customerMaterialPrice);
        }
    }
}
