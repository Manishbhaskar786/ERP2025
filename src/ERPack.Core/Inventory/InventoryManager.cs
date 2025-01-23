using Abp.Domain.Repositories;
using ERPack.Materials;
using ERPack.Materials.MaterialInventories;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Inventory
{
    public class InventoryManager: IInventoryManager
    {
        private readonly IRepository<InventoryIssued, long> _inventoryIssuedRepository;
        private readonly IRepository<InventoryIssuedItem, long> _inventoryIssuedItemRepository;
        public InventoryManager(
            IRepository<InventoryIssued, long> inventoryIssuedRepository,
            IRepository<InventoryIssuedItem, long> inventoryIssuedItemRepository)
        {
            _inventoryIssuedRepository = inventoryIssuedRepository;
            _inventoryIssuedItemRepository = inventoryIssuedItemRepository;
        }

        public async Task<long> IssueInventoryAsync(InventoryIssued inventoryIssued)
        {
            return await _inventoryIssuedRepository.InsertAndGetIdAsync(inventoryIssued);
        }

        public async Task<long> AddInventoryIssueItemAsync(InventoryIssuedItem inventoryIssuedItem)
        {
            return await _inventoryIssuedItemRepository.InsertAndGetIdAsync(inventoryIssuedItem);
        }

        public async Task<List<InventoryIssuedItem>> GetInventoryItemsAsync(long inventoryId)
        {
            return await _inventoryIssuedItemRepository.GetAll()
                .Include(x=> x.Department).Include(x => x.ItemType).Include(x => x.Material).Include(x => x.ToStore)
                .Include(x=> x.User).Include(x => x.FromStore)
                .Where(x=> x.InventoryIssueId == inventoryId).ToListAsync();
        }

        public async Task<List<InventoryIssued>> GetAllAsync()
        {
            return await _inventoryIssuedRepository.GetAllListAsync();
        }

    }
}
