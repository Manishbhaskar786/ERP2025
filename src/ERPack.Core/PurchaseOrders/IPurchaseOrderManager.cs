using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.PurchaseOrders
{
    public interface IPurchaseOrderManager : IDomainService
    {
        Task<PurchaseOrder> GetAsync(int id);
        Task<int> CreateAsync(PurchaseOrder purchaseOrder);
        Task<int> UpdateAsync(PurchaseOrder purchaseOrder);
        void Cancel(PurchaseOrder purchaseOrder);
        Task<List<PurchaseOrder>> GetAllAsync();
    }
}
