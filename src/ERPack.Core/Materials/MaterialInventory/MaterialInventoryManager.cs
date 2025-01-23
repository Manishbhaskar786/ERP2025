using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Materials.MaterialInventories
{
    public class MaterialInventoryManager : IMaterialInventoryManager
    {
        private readonly IRepository<MaterialInventory, int> _repository;

        public MaterialInventoryManager(
            IRepository<MaterialInventory, int> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(MaterialInventory input)
        {
            return await _repository.InsertAndGetIdAsync(input);
        }

        public async Task<MaterialInventory> UpdateAsync(MaterialInventory input)
        {
            return await _repository.UpdateAsync(input);
        }

        public Task<MaterialInventory> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                var department = _repository.GetAll().Where(x => x.Id == id).FirstOrDefault();

                if (department == null)
                {
                    throw new UserFriendlyException("Could not found the material inventory, maybe it's deleted!");
                }
                return department;
            });
        }

        public Task<MaterialInventory> GetMaterialInventoryByStoreAsync(int materialId, int storeId)
        {
            return Task.Run(() =>
            {
                var materialInventory = _repository.GetAll().Where(x => x.MaterialId == materialId && x.StoreId == storeId)
                .AsNoTracking().FirstOrDefault();
                return materialInventory;
            });
        }

        public async Task<List<MaterialInventory>> GetAllAsync()
        {
            var units = await _repository.GetAll().ToListAsync();

            if (units == null)
            {
                throw new UserFriendlyException("No Data found, please contact admin!");
            }
            return units;

        }

        public void Cancel(MaterialInventory input)
        {
            _repository.Delete(input);
        }
    }
}
