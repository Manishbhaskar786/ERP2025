using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Materials;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Inventory
{
    public class InventoryRequestManager : IInventoryRequestManager
    {
        private readonly IRepository<InventoryRequest, long> _inventoryRequestRepository;
        public InventoryRequestManager(
            IRepository<InventoryRequest, long> inventoryRequestRepository)
        {
            _inventoryRequestRepository = inventoryRequestRepository;
        }

        public async Task<long> CreateAsync(InventoryRequest inventoryRequest)
        {
            return await _inventoryRequestRepository.InsertAndGetIdAsync(inventoryRequest);
        }

        public async Task<List<InventoryRequest>> GetAllAsync()
        {
            return await _inventoryRequestRepository.GetAllListAsync();
        }

        public async Task<InventoryRequest> GetAsync(long id)
        {
            var inventoryRequest = await _inventoryRequestRepository.GetAll().Include(x=> x.User).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (inventoryRequest == null)
            {
                throw new UserFriendlyException("Could not found the Inventory Request, maybe it's deleted!");
            }
            return inventoryRequest;
        }

        public async Task<List<InventoryRequest>> GetActiveRequestsAsync()
        {
            return await _inventoryRequestRepository.GetAll().Where(x => x.IsReqClose != true).ToListAsync();
        }

        public async Task<InventoryRequest> GetByTaskId(long taskId)
        {
            return await _inventoryRequestRepository.GetAll().Where(x => x.WorkorderTaskId == taskId).FirstOrDefaultAsync();
        }

    }
}
