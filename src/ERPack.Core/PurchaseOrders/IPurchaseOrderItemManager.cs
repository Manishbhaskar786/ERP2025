using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.PurchaseOrders
{
    public interface IPurchaseOrderItemManager : IDomainService
    {
        Task<PurchaseOrderItem> GetAsync(int id);
        Task<int> UpdateAsync(PurchaseOrderItem purchaseOrderItem);
        Task<List<PurchaseOrderItem>> GetAllByPurchaseOrderIdAsync(int id);
        Task<int> CreateAsync(PurchaseOrderItem purchaseOrderItem);
        void Cancel(PurchaseOrderItem purchaseOrderItem);
        Task<List<PurchaseOrderItem>> GetAllAsync();
        Task<List<PurchaseOrderItem>> GetAllByPOCodeAsync(string poCode);
    }
}
