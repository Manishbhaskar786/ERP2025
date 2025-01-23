using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERPack.Preferences.Dto;
using ERPack.Stores.Dto;
using ERPack.Vendors.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Preferences
{
    public interface IPreferenceAppService : IApplicationService
    {
        Task<PagedResultDto<PreferenceDto>> GetAllAsync(PagedStoreResultRequestDto input);
        Task<int> CreateAsync(PreferenceDto input);
        Task<PreferenceDto> UpdateAsync(PreferenceDto input);
        Task<PreferenceDto> GetAsync(int storeId);
        Task<string> GetByNameAsync(string idType, string name = "");
        Task DeleteAsync(EntityDto<int> input);
    }
}
