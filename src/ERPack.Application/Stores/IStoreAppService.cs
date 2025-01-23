using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERPack.Stores.Dto;
using ERPack.Vendors.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Stores
{
    public interface IStoreAppService : IApplicationService
    {
        Task<PagedResultDto<StoreDto>> GetAllAsync(PagedStoreResultRequestDto input);
        Task<List<StoreDto>> GetAllStoresAsync();
        Task<int> CreateAsync(StoreDto input);
        Task<StoreDto> GetAsync(int storeId);
        Task DeleteAsync(EntityDto<int> input);
    }
}
