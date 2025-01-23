using Abp.Domain.Services;
using ERPack.PurchaseRecieves;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.PurchaseOrders
{
    public interface IPurchaseReceiveItemManager : IDomainService
    {
        Task<PurchaseReceiveItem> GetAsync(int id);
        Task<List<PurchaseReceiveItem>> GetAllByPurchaseRecieveIdAsync(int id);
        Task<int> CreateAsync(PurchaseReceiveItem purchaseReceiveItem);
        void Cancel(PurchaseReceiveItem purchaseReceiveItem);
        Task<List<PurchaseReceiveItem>> GetAllAsync();
    }
}
