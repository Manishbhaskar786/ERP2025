using Abp.Domain.Services;
using ERPack.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Inventory
{
    public interface IInventoryRequestManager : IDomainService
    {
        Task<long> CreateAsync(InventoryRequest inventoryRequest);
        Task<List<InventoryRequest>> GetAllAsync();
        Task<List<InventoryRequest>> GetActiveRequestsAsync();
        Task<InventoryRequest> GetByTaskId(long taskId);
        Task<InventoryRequest> GetAsync(long id);
    }
}
