using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERPack.Materials.Dto;
using ERPack.Materials.MaterialInventories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Materials
{
    public interface IMaterialAppService : IApplicationService
    {
        Task<long> CreateAsync(MaterialDto input);
        Task<MaterialDto> UpdateAsync(MaterialDto input);
        Task<MaterialDto> GetAsync(long cusotmerId);
        Task DeleteAsync(EntityDto<long> input);
        Task<List<MaterialDto>> GetAllMaterialsAsync();
        Task<MaterialInventoryDto> GetMaterialInventoryByStoreAsync(int materialId, int storeId);
        Task<int> AddMaterialInventoryAsync(MaterialInventoryDto input);
        Task<MaterialInventory> UpdateMaterialInventoryAsync(MaterialInventoryDto input);
        Task<List<MaterialDto>> GetAllByTypeAsync(int typeId);
    }
}
