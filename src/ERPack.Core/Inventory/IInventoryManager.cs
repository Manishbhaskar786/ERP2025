using Abp.Domain.Services;
using ERPack.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Inventory
{
    public interface IInventoryManager: IDomainService
    {
        Task<long> IssueInventoryAsync(InventoryIssued inventoryIssued);
        Task<long> AddInventoryIssueItemAsync(InventoryIssuedItem inventoryIssuedItem);
        Task<List<InventoryIssued>> GetAllAsync();
        Task<List<InventoryIssuedItem>> GetInventoryItemsAsync(long inventoryId);
    }
}
