using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Logging;
using Abp.UI;
using ERPack.Customers.Contacts;
using ERPack.Customers.CustomerTaxationInfos;
using ERPack.Customers.Dto;
using ERPack.Users.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Customers
{
    public class CustomerAppService : ERPackAppServiceBase, ICustomerAppService
    {
        private readonly IRepository<Customer, long> _customerRepository;
        private readonly IRepository<Contact, long> _contactRepository;
        private readonly IRepository<CustomerTaxationInfo, int> _taxationRepository;
        private readonly CustomerManager _customerManager;
        private readonly ContactManager _contactManager;
        private readonly CustomerTaxationManager _customerTaxationManager;
        private readonly IHostEnvironment _env;

        public CustomerAppService(IRepository<Customer, long> customerRepository,
            IRepository<Contact, long> contactRepository,
            IRepository<CustomerTaxationInfo, int> taxationRepository,
            CustomerManager customerManager,
            CustomerTaxationManager customerTaxationManager,
            ContactManager contactManager,
        IHostEnvironment env)
        {
            _customerRepository = customerRepository;
            _contactRepository = contactRepository;
            _taxationRepository = taxationRepository;
            _customerManager = customerManager;
            _contactManager = contactManager;
            _customerTaxationManager = customerTaxationManager;
            _env = env;
        }

        #region Customers Methods

        public async Task<(long, string)> CreateAsync(CustomerDto input)
        {
            try
            {
                var existPanNumber = await _customerRepository.FirstOrDefaultAsync(d => d.PAN == input.PAN);
                if (existPanNumber != null)
                {
                    throw new UserFriendlyException("PAN number already exist.");
                }

                var customer = ObjectMapper.Map<Customer>(input);
                if (input.ImageFile != null)
                {
                    var file = await SaveFile(input.ImageFile);
                    string fileName = Path.GetFileName(file);
                    customer.CompanyLogo = fileName;
                }

                long customerId = await _customerManager.CreateAsync(customer);
                return (customerId, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return (0, ex.Message);
            }
        }
        public async Task<(CustomerDto, string)> UpdateAsync(CustomerDto input)
        {
            var customer = new CustomerDto();
            try
            {
                var existPanNumber = await _customerRepository.FirstOrDefaultAsync(d => d.PAN == input.PAN && d.Id != input.Id);
                if (existPanNumber != null)
                {
                    throw new UserFriendlyException("PAN number already exist.");
                }
                var entity = await _customerRepository.GetAsync(input.Id);
                if (input.ImageFile != null)
                {
                    var file = await SaveFile(input.ImageFile);
                    string fileName = Path.GetFileName(file);
                    input.CompanyLogo = fileName;
                }
                MapToEntity(input, entity);
                var result = await _customerManager.UpdateAsync(entity);

                await CurrentUnitOfWork.SaveChangesAsync();

                customer = ObjectMapper.Map<CustomerDto>(result);

                return (customer, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return (customer, ex.Message);
            }
        }
        public async Task<CustomerDto> GetAsync(long cusotmerId)
        {
            var entity = await _customerManager.GetAsync(cusotmerId);
            var customer = ObjectMapper.Map<CustomerDto>(entity);
            return customer;
        }
        public async Task<CustomerDto> GetByIdAsync(long cusotmerId)
        {
            var entity = await _customerManager.GetByIdAsync(cusotmerId);
            var customer = ObjectMapper.Map<CustomerDto>(entity);
            return customer;
        }
        public async Task<PagedResultDto<CustomerDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);

            var customers = query
                .Skip(input.SkipCount)
                .ToList();

            var customerList = new List<CustomerDto>();
            foreach (var customer in customers)
            {
                var customerDto = ObjectMapper.Map<CustomerDto>(customer);
                var customerDefaultAddress = await _customerTaxationManager.GetDefaultTaxationByCustomerAsync(customer.Id);
                if (customerDefaultAddress == null)
                {
                    customerDto.City = customer.City;
                    customerDto.State = customer.State;
                }
                else
                {
                    customerDto.City = customerDefaultAddress.City;
                    customerDto.State = customerDefaultAddress.State;
                }
                customerList.Add(customerDto);
            }

            var result = new PagedResultDto<CustomerDto>(query.Count(), customerList);
            return result;
        }
        public async Task<PagedResultDto<CustomerDto>> GetCustomersListAsync()
        {
            var customers = await _customerRepository.GetAllListAsync();
            var customersList = new List<CustomerDto>(ObjectMapper.Map<List<CustomerDto>>(customers));

            return new PagedResultDto<CustomerDto>(
               totalCount: customersList.Count,
               items: customersList
           );
        }
        public async Task<List<CustomerNameOutput>> GetCustomersNamesAsync(string name)
        {
            try
            {
                var customers = await _customerManager.SearchCustomersByNameAsync(name);

                var customerNames = customers.Select(x => new CustomerNameOutput { Id = x.Id, Name = x.Name }).ToList();

                return customerNames;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }
        public async Task DeleteAsync(EntityDto<long> input)
        {
            try
            {
                var customer = await _customerManager.GetAsync(input.Id);
                _customerManager.Cancel(customer);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }

        }

        #endregion

        #region Contact Methods

        public async Task<ContactDto> GetContactAsync(long contactId)
        {
            var entity = await _contactManager.GetAsync(contactId);
            var contact = ObjectMapper.Map<ContactDto>(entity);
            return contact;
        }

        public Task<PagedResultDto<ContactDto>> GetAllContactAsync(PagedCustomerSubResultRequestDto input)
        {
            var customers = new List<Contact>();
            var query = CreateContactFilteredQuery(input);

            query = ApplyContactSorting(query, input);

            customers = query
                .Skip(input.SkipCount)
                .ToList();

            var result = new PagedResultDto<ContactDto>(query.Count(), ObjectMapper.Map<List<ContactDto>>(customers));
            return Task.FromResult(result);
        }

        public async Task<long> CreateContactAsync(ContactDto input)
        {
            try
            {
                var entity = await _contactManager.CheckUniquenessAsync(input.EmailAddress, input.Id);
                if (entity != null)
                {
                    throw new UserFriendlyException($"'{input.EmailAddress}' already taken");
                }
                var contact = ObjectMapper.Map<Contact>(input);
                long contactId = await _contactManager.CreateAsync(contact);
                return contactId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<ContactDto> UpdateContactAsync(ContactDto input)
        {
            var entity = await _contactRepository.GetAsync(input.Id);
            MapToContactEntity(input, entity);

            var result = await _contactManager.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            var contact = ObjectMapper.Map<ContactDto>(result);

            return contact;
        }

        #endregion

        #region Taxation Methods

        public async Task<CustomerTaxationInfoDto> GetTaxationAsync(long taxationId)
        {
            var entity = await _customerTaxationManager.GetAsync(taxationId);
            var taxation = ObjectMapper.Map<CustomerTaxationInfoDto>(entity);
            return taxation;
        }

        public Task<PagedResultDto<CustomerTaxationInfoDto>> GetAllTaxationAsync(PagedCustomerSubResultRequestDto input)
        {
            var customers = new List<CustomerTaxationInfo>();
            var query = CreateTaxationFilteredQuery(input);

            query = ApplyTaxationSorting(query, input);

            customers = query
                .Skip(input.SkipCount)
                .ToList();

            var result = new PagedResultDto<CustomerTaxationInfoDto>(query.Count(), ObjectMapper.Map<List<CustomerTaxationInfoDto>>(customers));
            return Task.FromResult(result);
        }

        public async Task<(long, string)> CreateTaxationAsync(CustomerTaxationInfoDto input)
        {
            try
            {
                var existingAddress = await _taxationRepository.GetAll().Where(x => x.CustomerId == input.CustomerId && x.IsDefault).FirstOrDefaultAsync();
                if (existingAddress != null && input.IsDefault)
                {
                    throw new UserFriendlyException("There is already a default address.");
                }
                var existPanNumber = await _taxationRepository.GetAll().Include(x => x.Customer).Where(x => x.PANNumber == input.PANNumber || x.Customer.PAN == input.PANNumber).FirstOrDefaultAsync();
                if (existPanNumber != null)
                {
                    throw new UserFriendlyException("Pan Number already exist.");
                }

                var taxation = ObjectMapper.Map<CustomerTaxationInfo>(input);
                long taxationId = await _customerTaxationManager.CreateAsync(taxation);
                return (taxationId, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return (0, ex.Message);
            }
        }

        public async Task<CustomerTaxationInfoDto> UpdateTaxationAsync(CustomerTaxationInfoDto input)
        {
            var existingAddress = await _taxationRepository.GetAll().Where(x => x.CustomerId == input.CustomerId && x.IsDefault && x.Id != input.Id).FirstOrDefaultAsync();
            if (existingAddress != null && input.IsDefault)
            {
                throw new UserFriendlyException("There is already a default address.");
            }
            var existPanNumber = await _taxationRepository.GetAll().Include(x => x.Customer).Where(x => (x.PANNumber == input.PANNumber || x.Customer.PAN == input.PANNumber) && x.Id != input.Id).FirstOrDefaultAsync();
            if (existPanNumber != null)
            {
                throw new UserFriendlyException("Pan Number already exist.");
            }
            var entity = await _taxationRepository.GetAsync(input.Id);
            MapToTaxationEntity(input, entity);

            var result = await _customerTaxationManager.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            var taxation = ObjectMapper.Map<CustomerTaxationInfoDto>(result);
            return taxation;
        }

        #endregion

        #region Protected Methods
        protected IQueryable<Customer> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return _customerRepository.GetAll().Include(x => x.Category).Include(x => x.BusinessCurrency).Include(x => x.State)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Contact> CreateContactFilteredQuery(PagedCustomerSubResultRequestDto input)
        {
            return _contactRepository.GetAll().AsQueryable().Where(x => x.CustomerId == input.CustomerId);
        }
        protected IQueryable<CustomerTaxationInfo> CreateTaxationFilteredQuery(PagedCustomerSubResultRequestDto input)
        {
            return _taxationRepository.GetAll().Include(x => x.State).Include(x => x.Country).Where(x => x.CustomerId == input.CustomerId).AsQueryable();
        }

        protected IQueryable<Customer> ApplySorting(IQueryable<Customer> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.Id);
        }

        protected IQueryable<Contact> ApplyContactSorting(IQueryable<Contact> query, PagedCustomerSubResultRequestDto input)
        {
            return query.OrderBy(r => r.Id);
        }

        protected IQueryable<CustomerTaxationInfo> ApplyTaxationSorting(IQueryable<CustomerTaxationInfo> query, PagedCustomerSubResultRequestDto input)
        {
            return query.OrderBy(r => r.Id);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var uniqueFileName = GetUniqueFileName(file.FileName);
            var dir = Path.Combine(_env.ContentRootPath, "wwwroot\\Customer");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filePath = Path.Combine(dir, uniqueFileName);
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            return Path.Combine(@"\Customer" + "\'", uniqueFileName);
        }

        protected void MapToEntity(CustomerDto input, Customer customer)
        {
            ObjectMapper.Map(input, customer);
        }

        protected void MapToContactEntity(ContactDto input, Contact contact)
        {
            ObjectMapper.Map(input, contact);
        }

        protected void MapToTaxationEntity(CustomerTaxationInfoDto input, CustomerTaxationInfo contact)
        {
            ObjectMapper.Map(input, contact);
        }

        #endregion
    }
}
