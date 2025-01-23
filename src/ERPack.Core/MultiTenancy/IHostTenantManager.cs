using Abp.Domain.Services;
using ERPack.Customers;
using ERPack.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.MultiTenancy
{
    public interface IHostTenantManager : IDomainService
    {
        Task<HostTenantInfo> GetAsync();
        Task<int> CreateAsync(HostTenantInfo material);
        Task<HostTenantInfo> UpdateAsync(HostTenantInfo material);
        Task<List<HostTenantInfo>> GetAllAsync();
    }
}
