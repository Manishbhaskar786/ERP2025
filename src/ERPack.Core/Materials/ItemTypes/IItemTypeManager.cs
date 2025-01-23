using Abp.Domain.Services;
using ERPack.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Materials.ItemTypes
{
    public interface IItemTypeManager : IDomainService
    {
        Task<ItemType> GetAsync(int id);
        Task<int> CreateAsync(ItemType itemType);
        void Cancel(ItemType itemType);
        Task<List<ItemType>> GetAllAsync();
    }
}
