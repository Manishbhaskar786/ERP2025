using Abp.Application.Services;
using ERPack.PurchaseReceives.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.PurchaseRecieves
{
    public interface IPurchaseReceiveAppService : IApplicationService
    {
        Task<int> CreateAsync(PurchaseReceiveDto input);
        Task<int> CreatePurchaseReceiveItemAsync(PurchaseReceiveItemDto input);
        Task<List<PurchaseReceiveItemDto>> GetPurchaseReceiveItemsAsync(int id);
    }
}
