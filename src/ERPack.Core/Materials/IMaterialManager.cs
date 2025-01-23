using Abp.Domain.Services;
using ERPack.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Materials
{
    public interface IMaterialManager : IDomainService
    {
        Task<Material> GetAsync(long id);
        Task<long> CreateAsync(Material material);
        Task<Material> UpdateAsync(Material material);
        void Cancel(Material material);
        Task<List<Material>> GetAllAsync();
        Task<List<Material>> GetAllByTypeAsync(int typeId);
    }
}
