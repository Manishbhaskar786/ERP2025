using Abp.Domain.Services;
using ERPack.PurchaseRecieves;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.PurchaseReceives
{
    public interface IPurchaseReceiveManager : IDomainService
    {
        Task<PurchaseReceive> GetAsync(int id);
        Task<int> CreateAsync(PurchaseReceive purchaseReceive);
        void Cancel(PurchaseReceive purchaseReceive);
        Task<List<PurchaseReceive>> GetAllAsync();
    }
}
