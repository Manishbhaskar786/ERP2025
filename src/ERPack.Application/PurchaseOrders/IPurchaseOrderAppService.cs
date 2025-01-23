using Abp.Application.Services;
using ERPack.PurchaseOrders.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.PurchaseOrders
{
    public interface IPurchaseOrderAppService : IApplicationService
    {
        Task<int> CreateAsync(PurchaseOrderDto input);
        Task<int> UpdateAsync(PurchaseOrderDto input);
        Task<List<PurchaseOrderDto>> GetAllPurchaseOrdersAsync();
        Task<List<PurchaseOrderItemDto>> GetPurchaseOrderItemsAsync(int id);
        Task<int> CreatePurchaseOrderItemAsync(PurchaseOrderItemDto input);
        Task<int> updatePurchaseOrderItemAsync(PurchaseOrderItemDto input);
        Task<List<PurchaseOrderItemDto>> GetAllByPOCodeAsync(string poCode);
        Task<PurchaseOrderDto> GetByIdAsync(int id);
    }
}
