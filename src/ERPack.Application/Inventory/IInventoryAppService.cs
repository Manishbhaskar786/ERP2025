using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERPack.Common.Dto;
using ERPack.Departments.Dto;
using ERPack.Inventory.Dto;
using ERPack.Units.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Inventory
{
    public interface IInventoryAppService : IApplicationService
    {
        Task<PagedResultDto<InventoryIssuedDto>> GetAllAsync(CommonPagedResultRequestDto input);
        Task<long> IssueInventoryAsync(InventoryIssuedDto input);
        Task<InventoryRequestOutput> GetInventoryRequestAsync(long id);
        Task<List<InventoryRequestOutput>> GetInventoryRequestsAsync();
        Task<long> AddInventoryItemAsync(InventoryIssuedItemDto input);
        Task<List<InventoryIssuedItemDto>> GetInventoryItemsAsync(long inventoryId);
        Task<long> InventoryRequestAsync(InventoryRequestDto input);
    }
}
