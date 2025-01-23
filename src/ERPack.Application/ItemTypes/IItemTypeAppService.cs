using Abp.Application.Services;
using ERPack.Departments.Dto;
using ERPack.Units.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Departments
{
    public interface IItemTypeAppService : IApplicationService
    {
        Task<List<ItemTypeOutput>> GetItemTypesAsync();
        Task<int> CreateItemTypeAsync(ItemTypeDto input);
    }
}
