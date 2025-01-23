using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Materials.MaterialInventories
{
    public interface IMaterialInventoryManager : IDomainService
    {
        Task<MaterialInventory> GetAsync(int id);
        Task<int> CreateAsync(MaterialInventory input);
        Task<MaterialInventory> GetMaterialInventoryByStoreAsync(int materialId, int storeId);
        void Cancel(MaterialInventory input);
        Task<List<MaterialInventory>> GetAllAsync();
    }
}
