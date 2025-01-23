using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.MultiTenancy
{
    public interface ITenantBgService : IApplicationService
    {
        Task<List<Tenant>> GetAllAsync();
        Task DeleteTenantAsync(int tenantId);
    }
}
