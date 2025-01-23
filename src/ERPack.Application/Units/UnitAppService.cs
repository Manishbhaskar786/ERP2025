using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using ERPack.Departments;
using ERPack.Departments.Dto;
using ERPack.Materials.Units;
using ERPack.Units.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Units
{
    [AbpAuthorize]
    public class UnitAppService : ERPackAppServiceBase , IUnitAppService
    {
        private readonly UnitManager _unitManager;

        public UnitAppService(IRepository<Unit> repository,
            UnitManager unitManager)
        {
            _unitManager = unitManager;
        }

        public  async Task<int> CreateUnitAsync(UnitDto input)
        {

            var unit = ObjectMapper.Map<Unit>(input);

            int unitId =  await _unitManager.CreateAsync(unit);

            return unitId;
        }

        public async Task<List<UnitOutput>> GetUnitsAsync()
        {
            var units = await _unitManager.GetAllAsync();

            var result = ObjectMapper.Map<List<UnitOutput>>(units);

            return result;
        }

    }
}
