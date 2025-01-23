using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Enquiries;
using ERPack.Materials;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Designs.DesignMaterials
{
    public class DesignMaterialManager : IDesignMaterialManager
    {
        private IRepository<DesignMaterial, int> _designMaterialRepository;

        public DesignMaterialManager(IRepository<DesignMaterial, int> designMaterialRepository)
        {
            _designMaterialRepository = designMaterialRepository;
        }


        public async Task<long> CreateAsync(DesignMaterial designMaterial)
        {
            try
            {
                return await _designMaterialRepository.InsertAndGetIdAsync(designMaterial);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error Creating Design Material!", ex.Message);
            }

        }

        public async Task<DesignMaterial> UpdateAsync(DesignMaterial designMaterial)
        {
            return await _designMaterialRepository.UpdateAsync(designMaterial);
        }

        public async Task<DesignMaterial> GetAsync(int id)
        {
            var designMaterial = await _designMaterialRepository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (designMaterial == null)
            {
                throw new UserFriendlyException("Could not found the design, maybe it's deleted!");
            }
            return designMaterial;
        }

        public async Task<List<DesignMaterial>> GetDesignMaterialsAsync(long id)
        {
            var designMaterials = await _designMaterialRepository.GetAllIncluding(x=> x.Material).Where(x => x.DesignId == id).ToListAsync();

            if (designMaterials == null)
            {
                throw new UserFriendlyException("Could not found the design materials, maybe it's deleted!");
            }
            return designMaterials;
        }

        public async Task<List<DesignMaterial>> GetAllAsync()
        {
            var designMaterials = await _designMaterialRepository.GetAll().ToListAsync();

            if (designMaterials == null)
            {
                throw new UserFriendlyException("No design materials found, please contact admin!");
            }
            return designMaterials;
        }

        public void Cancel(DesignMaterial designMaterial)
        {
            _designMaterialRepository.Delete(designMaterial);
        }

        public async Task DeleteDesignMaterialByDesignIdsAsync(long designId)
        {
            var designMaterials = await _designMaterialRepository.GetAll().Where(x => x.DesignId == designId).ToListAsync();

            if (designMaterials != null)
            {
                foreach (var item in designMaterials)
                {
                    _designMaterialRepository.Delete(item.Id);
                }
            }
        }
    }
}
