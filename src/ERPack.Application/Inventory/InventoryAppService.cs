using Abp.Application.Services.Dto;
using Abp.Authorization;
using ERPack.Common.Dto;
using ERPack.Estimates.Dto;
using ERPack.Estimates;
using ERPack.Inventory.Dto;
using ERPack.Units.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Extensions;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Collections.Extensions;
using System;
using Abp.UI;

namespace ERPack.Inventory
{
    [AbpAuthorize]
    public class InventoryAppService : ERPackAppServiceBase, IInventoryAppService
    {
        private readonly InventoryManager _inventoryManager;
        private readonly InventoryRequestManager _inventoryRequestManager;
        private readonly IRepository<InventoryIssued, long> _inventoryIssuedRepository;

        public InventoryAppService(InventoryManager inventoryManager,
            InventoryRequestManager inventoryRequestManager,
            IRepository<InventoryIssued, long> inventoryIssuedRepository)
        {
            _inventoryManager = inventoryManager;
            _inventoryRequestManager = inventoryRequestManager;
            _inventoryIssuedRepository = inventoryIssuedRepository;
        }

        public async Task<long> IssueInventoryAsync(InventoryIssuedDto input)
        {

            var inventoryIssued = ObjectMapper.Map<InventoryIssued>(input);

            long inventoryIssuedId = await _inventoryManager.IssueInventoryAsync(inventoryIssued);

            return inventoryIssuedId;
        }

        public async Task<long> AddInventoryItemAsync(InventoryIssuedItemDto input)
        {

            var inventoryIssuedItem = ObjectMapper.Map<InventoryIssuedItem>(input);

            long inventoryIssuedItemId = await _inventoryManager.AddInventoryIssueItemAsync(inventoryIssuedItem);

            return inventoryIssuedItemId;
        }

        public async Task<InventoryRequestOutput> GetInventoryRequestAsync(long id)
        {
            var inventoryRequest = await _inventoryRequestManager.GetAsync(id);

            var result = ObjectMapper.Map<InventoryRequestOutput>(inventoryRequest);

            return result;
        }

        public async Task<List<InventoryRequestOutput>> GetInventoryRequestsAsync()
        {
            var inventoryRequests = await _inventoryRequestManager.GetActiveRequestsAsync();

            var result = ObjectMapper.Map<List<InventoryRequestOutput>>(inventoryRequests);

            return result;
        }

        public async Task<List<InventoryIssuedItemDto>> GetInventoryItemsAsync(long inventoryId)
        {
            var inventoryItems = await _inventoryManager.GetInventoryItemsAsync(inventoryId);

            var result = ObjectMapper.Map<List<InventoryIssuedItemDto>>(inventoryItems);

            return result;
        }

        public Task<PagedResultDto<InventoryIssuedDto>> GetAllAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<InventoryIssued> inventory = query
                .Skip(input.SkipCount)
               // .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<InventoryIssuedDto>(query.Count(), ObjectMapper.Map<List<InventoryIssuedDto>>(inventory));
            return Task.FromResult(result);
        }

        public async Task<long> InventoryRequestAsync(InventoryRequestDto input)
        {
            try
            {
                var inventoryRequest = ObjectMapper.Map<InventoryRequest>(input);

                long inventoryRequestId = await _inventoryRequestManager.CreateAsync(inventoryRequest);

                return inventoryRequestId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }


        protected IQueryable<InventoryIssued> CreateFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _inventoryIssuedRepository.GetAllIncluding(x => x.User)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.IssueCode.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<InventoryIssued> ApplySorting(IQueryable<InventoryIssued> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
