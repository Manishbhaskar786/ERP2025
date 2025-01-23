using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using ERPack.Authorization.Users;
using ERPack.Customers;
using ERPack.Customers.Dto;
using ERPack.Stores;
using ERPack.Stores.Dto;
using ERPack.Vendors.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Stores
{
    [AbpAuthorize]
    public class StoreAppService : ERPackAppServiceBase, IStoreAppService
    {
        IRepository<Store, int> _storeRepository;
        private readonly StoreManager _storeManager;

        public StoreAppService(IRepository<Store> repository,
            StoreManager storeManager)
        {
            _storeRepository = repository;
            _storeManager = storeManager;
        }

        public async Task<int> CreateAsync(StoreDto input)
        {

            var store = ObjectMapper.Map<Store>(input);

            int storeId = await _storeManager.CreateAsync(store);

            return storeId;
        }

        public async Task<StoreDto> UpdateAsync(StoreDto input)
        {
            var entity = await _storeRepository.GetAsync(input.Id);

            MapToEntity(input, entity);

            var result = await _storeManager.UpdateAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            var store = ObjectMapper.Map<StoreDto>(result);

            return store;
        }

        public Task<PagedResultDto<StoreDto>> GetAllAsync(PagedStoreResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<Store> stores = query
                .Skip(input.SkipCount)
               // .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<StoreDto>(query.Count(), ObjectMapper.Map<List<StoreDto>>(stores));
            return Task.FromResult(result);
        }

        public async Task<List<StoreDto>> GetAllStoresAsync()
        {
            var stores = await _storeManager.GetAllAsync();

            var result = ObjectMapper.Map<List<StoreDto>>(stores);

            return result;
        }

        public async Task<StoreDto> GetAsync(int storeId)
        {
            var entity = await _storeManager.GetAsync(storeId);
            var store = ObjectMapper.Map<StoreDto>(entity);
            return store;
        }

        public async Task DeleteAsync(EntityDto<int> input)
        {
            try
            {
                var store = await _storeManager.GetAsync(input.Id);
                _storeManager.Cancel(store);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }

        }

        protected IQueryable<Store> CreateFilteredQuery(PagedStoreResultRequestDto input)
        {
            return _storeRepository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.StoreName.Contains(input.Keyword) || x.StoreLocation.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Store> ApplySorting(IQueryable<Store> query, PagedStoreResultRequestDto input)
        {
            return query.OrderBy(r => r.StoreName);
        }

        protected void MapToEntity(StoreDto input, Store store)
        {
            ObjectMapper.Map(input, store);
        }

    }
}
