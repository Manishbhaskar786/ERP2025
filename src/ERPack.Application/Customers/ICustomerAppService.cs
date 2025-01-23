using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERPack.Customers.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Customers
{
    public interface ICustomerAppService : IApplicationService
    {
        Task<(long, string)> CreateAsync(CustomerDto input);
        Task<(CustomerDto, string)> UpdateAsync(CustomerDto input);
        Task<CustomerDto> GetAsync(long cusotmerId);
        Task<CustomerDto> GetByIdAsync(long cusotmerId);
        Task<PagedResultDto<CustomerDto>> GetCustomersListAsync();
        Task DeleteAsync(EntityDto<long> input);
        Task<List<CustomerNameOutput>> GetCustomersNamesAsync(string name);
        //Task<long> CreateCustomerMaterialPriceAsync(CustomerMaterialPriceDto input);
        //Task<List<CustomerMaterialPriceDto>> GetCustomerMaterialPricesAsync(long customerId);
        //Task DeleteCustomerMaterialPriceAsync(EntityDto<long> input);

        Task<ContactDto> GetContactAsync(long contactId);
        Task<long> CreateContactAsync(ContactDto input);
        Task<ContactDto> UpdateContactAsync(ContactDto input);

        Task<CustomerTaxationInfoDto> GetTaxationAsync(long contactId);
        Task<(long, string)> CreateTaxationAsync(CustomerTaxationInfoDto input);
        Task<CustomerTaxationInfoDto> UpdateTaxationAsync(CustomerTaxationInfoDto input);
    }
}
