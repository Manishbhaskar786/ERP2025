using Abp.Application.Services;
using ERPack.MultiTenancy.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
        Task<HostTenantInfo> GetHostTenantInfoAsync();
        Task<TenantDto> UpdateTenantAsync(TenantDto input);
        //Task<List<Tenant>> GetAllAsync();
        //Task DeleteTenantAsync(int tenantId);
    }
}

