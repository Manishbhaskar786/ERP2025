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
    public interface IUnitAppService : IApplicationService
    {
        Task<List<UnitOutput>> GetUnitsAsync();
        Task<int> CreateUnitAsync(UnitDto input);
    }
}
