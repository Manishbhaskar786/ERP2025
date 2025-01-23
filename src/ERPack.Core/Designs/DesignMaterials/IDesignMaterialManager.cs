using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Designs.DesignMaterials
{
    public interface IDesignMaterialManager : IDomainService
    {
        Task<long> CreateAsync(DesignMaterial designMaterial);
        Task<DesignMaterial> UpdateAsync(DesignMaterial designMaterial);
        Task<DesignMaterial> GetAsync(int id);
        Task<List<DesignMaterial>> GetDesignMaterialsAsync(long id);
        Task<List<DesignMaterial>> GetAllAsync();
        void Cancel(DesignMaterial design);
    }
}
