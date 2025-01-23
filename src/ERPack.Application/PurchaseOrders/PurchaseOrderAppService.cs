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
using ERPack.Materials.Dto;
using Abp.Logging;
using ERPack.Materials;
using Abp.UI;

namespace ERPack.PurchaseOrders
{
    [AbpAuthorize(PermissionNames.Pages_PurchaseOrder)]
    public class PurchaseOrderAppService : ERPackAppServiceBase, IPurchaseOrderAppService
    {
        readonly IRepository<PurchaseOrder, int> _purchaseOrderRepository;
        private readonly PurchaseOrderManager _purchaseOrderManager;
        private readonly PurchaseOrderItemManager _purchaseOrderItemManager;

        public PurchaseOrderAppService(IRepository<PurchaseOrder, int> purchaseOrderRepository,
            PurchaseOrderManager purchaseOrderManager,
            PurchaseOrderItemManager purchaseOrderItemManager)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderManager = purchaseOrderManager;
            _purchaseOrderItemManager = purchaseOrderItemManager;
        }

        public async Task<int> CreateAsync(PurchaseOrderDto input)
        {
            try
            {
                var purchaseOrder = ObjectMapper.Map<PurchaseOrder>(input);

                int purchaseOrderId = await _purchaseOrderManager.CreateAsync(purchaseOrder);

                return purchaseOrderId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<int> UpdateAsync(PurchaseOrderDto input)
        {
            try
            {
                var purchaseOrder = ObjectMapper.Map<PurchaseOrder>(input);

                int purchaseOrderId = await _purchaseOrderManager.UpdateAsync(purchaseOrder);

                return purchaseOrderId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task DeleteAsync(EntityDto<int> input)
        {
            try
            {
                var customer = await _purchaseOrderManager.GetAsync(input.Id);
                _purchaseOrderManager.Cancel(customer);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }

        }

        public Task<PagedResultDto<PurchaseOrderDto>> GetAllAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<PurchaseOrder> purchaseOrders = query
                .Skip(input.SkipCount)
               // .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<PurchaseOrderDto>(query.Count(), ObjectMapper.Map<List<PurchaseOrderDto>>(purchaseOrders));
            return Task.FromResult(result);
        }

        public async Task<List<PurchaseOrderDto>> GetAllPurchaseOrdersAsync()
        {
            var purchaseOrders = await _purchaseOrderManager.GetAllAsync();

            var result = ObjectMapper.Map<List<PurchaseOrderDto>>(purchaseOrders);

            return result;
        }

        public async Task<int> CreatePurchaseOrderItemAsync(PurchaseOrderItemDto input)
        {
            try
            {
                var purchaseOrderItem = ObjectMapper.Map<PurchaseOrderItem>(input);

                int purchaseOrderItemId = await _purchaseOrderItemManager.CreateAsync(purchaseOrderItem);

                return purchaseOrderItemId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<int> updatePurchaseOrderItemAsync(PurchaseOrderItemDto input)
        {
            try
            {
                var purchaseOrderItem = ObjectMapper.Map<PurchaseOrderItem>(input);

                int purchaseOrderItemId = await _purchaseOrderItemManager.UpdateAsync(purchaseOrderItem);

                return purchaseOrderItemId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<List<PurchaseOrderItemDto>> GetPurchaseOrderItemsAsync(int id)
        {
            var purchaseOrderItems = await _purchaseOrderItemManager.GetAllByPurchaseOrderIdAsync(id);

            var result = ObjectMapper.Map<List<PurchaseOrderItemDto>>(purchaseOrderItems);

            return result;
        }

        public async Task<List<PurchaseOrderItemDto>> GetAllByPOCodeAsync(string poCode)
        {
            var purchaseOrderItems = await _purchaseOrderItemManager.GetAllByPOCodeAsync(poCode);

            var result = ObjectMapper.Map<List<PurchaseOrderItemDto>>(purchaseOrderItems);

            return result;
        }
        public async Task<PurchaseOrderDto> GetByIdAsync(int id)
        {
            var purchaseOrders = await _purchaseOrderManager.GetAsync(id);
            var result = ObjectMapper.Map<PurchaseOrderDto>(purchaseOrders);

            return result;
        }

        protected IQueryable<PurchaseOrder> CreateFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _purchaseOrderRepository.GetAllIncluding(x=> x.Vendor)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.POCode.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<PurchaseOrder> ApplySorting(IQueryable<PurchaseOrder> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }
      
    }
}
