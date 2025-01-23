using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Departments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Materials.ItemTypes
{
    public class ItemTypeManager : IItemTypeManager
    {
        private readonly IRepository<ItemType, int> _itemTypeRepository;

        public ItemTypeManager(
            IRepository<ItemType, int> itemTypeRepository)
        {
            _itemTypeRepository = itemTypeRepository;
        }

        public async Task<int> CreateAsync(ItemType itemType)
        {
            return await _itemTypeRepository.InsertAndGetIdAsync(itemType);

        }

        public Task<ItemType> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                var itemType = _itemTypeRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();

                if (itemType == null)
                {
                    throw new UserFriendlyException("Could not found the item type, maybe it's deleted!");
                }
                return itemType;
            });
        }

        public async Task<List<ItemType>> GetAllAsync()
        {
            var units = await _itemTypeRepository.GetAll().ToListAsync();

            if (units == null)
            {
                throw new UserFriendlyException("No Item Types found, please contact admin!");
            }
            return units;

        }

        public void Cancel(ItemType unit)
        {
            _itemTypeRepository.Delete(unit);
        }
    }
}
