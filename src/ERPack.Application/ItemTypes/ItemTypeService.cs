using Abp.Authorization;
using Abp.Domain.Repositories;
using ERPack.Departments;
using ERPack.Materials.ItemTypes;
using ERPack.Units.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.ItemTypes
{
    [AbpAuthorize]
    public class ItemTypeAppService : ERPackAppServiceBase , IItemTypeAppService
    {
        private readonly ItemTypeManager _itemTypeManager;

        public ItemTypeAppService(IRepository<ItemType> repository,
            ItemTypeManager itemTypeManager)
        {
            _itemTypeManager = itemTypeManager;
        }

        public  async Task<int> CreateItemTypeAsync(ItemTypeDto input)
        {

            var itemType = ObjectMapper.Map<ItemType>(input);

            int itemTypeId =  await _itemTypeManager.CreateAsync(itemType);

            return itemTypeId;
        }

        public async Task<List<ItemTypeOutput>> GetItemTypesAsync()
        {
            var itemTypes = await _itemTypeManager.GetAllAsync();

            var result = ObjectMapper.Map<List<ItemTypeOutput>>(itemTypes);

            return result;
        }

    }
}
