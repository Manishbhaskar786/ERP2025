using Abp.Domain.Services;
using ERPack.Customers;
using ERPack.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.HostTaxation
{
    public interface IHostTaxationManager : IDomainService
    {
        Task<HostTaxationInfo> GetAsync(int hostTaxationId);
        Task<int> CreateAsync(HostTaxationInfo material);
        Task<HostTaxationInfo> UpdateAsync(HostTaxationInfo material);
        Task<List<HostTaxationInfo>> GetAllAsync();
    }
}
