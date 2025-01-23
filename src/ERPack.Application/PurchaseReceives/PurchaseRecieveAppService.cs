using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using ERPack.Authorization;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using ERPack.PurchaseOrders.Dto;
using ERPack.Common.Dto;
using Abp.Collections.Extensions;
using ERPack.PurchaseRecieves;
using ERPack.PurchaseOrders;
using ERPack.PurchaseReceives.Dto;
using Abp.Logging;
using Microsoft.EntityFrameworkCore;

namespace ERPack.PurchaseReceives
{
    [AbpAuthorize(PermissionNames.Pages_PurchaseReceive)]
    public class PurchaseReceiveAppService : ERPackAppServiceBase, IPurchaseReceiveAppService
    {
        readonly IRepository<PurchaseReceive, int> _purchaseReceiveRepository;
        private readonly PurchaseReceiveManager _purchaseReceiveManager;
        private readonly PurchaseReceiveItemManager _purchaseReceiveItemManager;

        public PurchaseReceiveAppService(IRepository<PurchaseReceive, int> purchaseReceiveRepository,
            PurchaseReceiveItemManager purchaseReceiveItemManager,
            PurchaseReceiveManager purchaseReceiveManager)
        {
            _purchaseReceiveRepository = purchaseReceiveRepository;
            _purchaseReceiveItemManager = purchaseReceiveItemManager;
            _purchaseReceiveManager = purchaseReceiveManager;
        }

        public async Task<int> CreateAsync(PurchaseReceiveDto input)
        {
            try
            {
                var purchaseReceive = ObjectMapper.Map<PurchaseReceive>(input);

                int purchaseReceiveId = await _purchaseReceiveManager.CreateAsync(purchaseReceive);

                return purchaseReceiveId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public Task<PagedResultDto<PurchaseReceiveDto>> GetAllAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<PurchaseReceive> purchaseReceives = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<PurchaseReceiveDto>(query.Count(), ObjectMapper.Map<List<PurchaseReceiveDto>>(purchaseReceives));
            return Task.FromResult(result);
        }

        public async Task<int> CreatePurchaseReceiveItemAsync(PurchaseReceiveItemDto input)
        {
            try
            {
                var purchaseReceiveItem = ObjectMapper.Map<PurchaseReceiveItem>(input);

                int purchaseReceiveItemId = await _purchaseReceiveItemManager.CreateAsync(purchaseReceiveItem);

                return purchaseReceiveItemId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<List<PurchaseReceiveItemDto>> GetPurchaseReceiveItemsAsync(int id)
        {
            var purchaseReceiveItems = await _purchaseReceiveItemManager.GetAllByPurchaseRecieveIdAsync(id);

            var result = ObjectMapper.Map<List<PurchaseReceiveItemDto>>(purchaseReceiveItems);

            return result;
        }

        protected IQueryable<PurchaseReceive> CreateFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _purchaseReceiveRepository.GetAll().Include(x=> x.PurchaseOrder).Include(x=> x.Vendor)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.PurchaseOrder.POCode.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<PurchaseReceive> ApplySorting(IQueryable<PurchaseReceive> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }

    }
}
