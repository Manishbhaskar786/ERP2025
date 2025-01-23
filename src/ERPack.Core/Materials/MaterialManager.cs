using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Customers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Materials
{
    public class MaterialManager : IMaterialManager
    {
        private readonly IRepository<Material, int> _materialRepository;

        public MaterialManager(
            IRepository<Material, int> materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<long> CreateAsync(Material material)
        {
            return await _materialRepository.InsertAndGetIdAsync(material);
        }

        public async Task<Material> UpdateAsync(Material material)
        {
            return await _materialRepository.UpdateAsync(material);
        }

        public async Task<Material> GetAsync(long id)
        {
            var material = await _materialRepository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (material == null)
            {
                throw new UserFriendlyException("Could not found the material, maybe it's deleted!");
            }
            return material;
        }

        public async Task<List<Material>> GetAllByTypeAsync(int typeId)
        {
            var materials = await _materialRepository.GetAll().Where(x => x.ItemTypeId == typeId).ToListAsync();

            if (materials == null)
            {
                throw new UserFriendlyException("Could not found the materials, maybe it's deleted!");
            }
            return materials;
        }

        public async Task<List<Material>> GetAllAsync()
        {
            var materials = await _materialRepository.GetAll().ToListAsync();

            if (materials == null)
            {
                throw new UserFriendlyException("No material found, please contact admin!");
            }
            return materials;

        }

        public void Cancel(Material material)
        {
            _materialRepository.Delete(material);
        }
    }
}
