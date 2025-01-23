using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Logging;
using Abp.UI;
using ERPack.Authorization;
using ERPack.Inventory.Dto;
using ERPack.Inventory;
using ERPack.Materials.Dto;
using ERPack.Materials.MaterialInventories;
using ERPack.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPack.Customers.Dto;
using ERPack.Customers;

namespace ERPack.Materials
{
    [AbpAuthorize(PermissionNames.Pages_Materials)]
    public class MaterialAppService : ERPackAppServiceBase, IMaterialAppService
    {
        readonly IRepository<Material, int> _materialRepository;
        private readonly MaterialManager _materialManager;
        private readonly MaterialInventoryManager _materialInventoryManager;

        public MaterialAppService(IRepository<Material, int> materialRepository,
            MaterialManager materialManager,
            MaterialInventoryManager materialInventoryManager)
        {
            _materialRepository = materialRepository;
            _materialManager = materialManager;
            _materialInventoryManager = materialInventoryManager;
        }

        #region Material Methods

        public async Task<long> CreateAsync(MaterialDto input)
        {
            try
            {
                var material = ObjectMapper.Map<Material>(input);

                long materialId = await _materialManager.CreateAsync(material);

                return materialId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<MaterialDto> UpdateAsync(MaterialDto input)
        {
            var entity = await _materialRepository.GetAsync(input.Id);

            MapToEntity(input, entity);

            var result = await _materialManager.UpdateAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            var material = ObjectMapper.Map<MaterialDto>(result);

            return material;
        }

        public async Task<MaterialDto> GetAsync(long materialId)
        {
            var entity = await _materialManager.GetAsync(materialId);
            var material = ObjectMapper.Map<MaterialDto>(entity);
            return material;
        }

        public Task<PagedResultDto<MaterialDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<Material> materials = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<MaterialDto>(query.Count(), ObjectMapper.Map<List<MaterialDto>>(materials));
            return Task.FromResult(result);
        }

        public async Task<List<MaterialDto>> GetAllMaterialsAsync()
        {
            var materials = await _materialManager.GetAllAsync();

            var result = ObjectMapper.Map<List<MaterialDto>>(materials);

            return result;
        }

        public async Task<List<MaterialDto>> GetAllByTypeAsync(int typeId)
        {
            var materials = await _materialManager.GetAllByTypeAsync(typeId);

            var result = ObjectMapper.Map<List<MaterialDto>>(materials);

            return result;
        }

        public async Task DeleteAsync(EntityDto<long> input)
        {
            try
            {
                var customer = await _materialManager.GetAsync(input.Id);
                _materialManager.Cancel(customer);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }

        }

        #endregion

        #region Material Inventory Methods

        public async Task<MaterialInventoryDto> GetMaterialInventoryByStoreAsync(int materialId, int storeId)
        {
            var entity = await _materialInventoryManager.GetMaterialInventoryByStoreAsync(materialId, storeId);
            var materialInventory = ObjectMapper.Map<MaterialInventoryDto>(entity);
            return materialInventory;
        }

        public async Task<int> AddMaterialInventoryAsync(MaterialInventoryDto input)
        {
            var materialInventory = ObjectMapper.Map<MaterialInventory>(input);

            var materialInventoryId = await _materialInventoryManager.CreateAsync(materialInventory);

            return materialInventoryId;
        }

        public async Task<MaterialInventory> UpdateMaterialInventoryAsync(MaterialInventoryDto input)
        {
            var materialInventory = ObjectMapper.Map<MaterialInventory>(input);
            materialInventory.CreatorUserId = AbpSession.UserId;

            var result = await _materialInventoryManager.UpdateAsync(materialInventory);

            return result;
        }

        #endregion

        #region Private Methods
        protected IQueryable<Material> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return _materialRepository.GetAllIncluding(x => x.ItemType, x => x.SellingUnit, x => x.BuyingUnit, x=> x.Department)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.ItemCode.Contains(input.Keyword) ||
                x.DisplayName.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Material> ApplySorting(IQueryable<Material> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.MaterialName);
        }
        protected void MapToEntity(MaterialDto input, Material material)
        {
            ObjectMapper.Map(input, material);
        }

        #endregion
    }
}
