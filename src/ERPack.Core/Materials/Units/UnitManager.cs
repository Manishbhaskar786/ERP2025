using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Departments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Materials.Units
{
    public class UnitManager : IUnitManager
    {
        private readonly IRepository<Unit, int> _unitRepository;

        public UnitManager(
            IRepository<Unit, int> unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<int> CreateAsync(Unit unit)
        {
            return await _unitRepository.InsertAndGetIdAsync(unit);

        }

        public Task<Unit> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                var department = _unitRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();

                if (department == null)
                {
                    throw new UserFriendlyException("Could not found the unit, maybe it's deleted!");
                }
                return department;
            });
        }

        public async Task<List<Unit>> GetAllAsync()
        {
            var units = await _unitRepository.GetAll().ToListAsync();

            if (units == null)
            {
                throw new UserFriendlyException("No Units found, please contact admin!");
            }
            return units;

        }

        public void Cancel(Unit unit)
        {
            _unitRepository.Delete(unit);
        }
    }
}
