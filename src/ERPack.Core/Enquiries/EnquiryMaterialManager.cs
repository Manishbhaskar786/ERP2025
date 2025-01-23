using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.UI;
using ERPack.Designs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Enquiries;
public class EnquiryMaterialManager : IEnquiryMaterialManager
{
    private readonly IRepository<EnquiryMaterial, int> _repository;

    public EnquiryMaterialManager(IRepository<EnquiryMaterial, int> repository)
    {
        _repository = repository;
    }
    public async Task<List<EnquiryMaterial>> GetAllByEnquiryIdAsync(int enquiryid)
    {
        var enquiryMaterials = await _repository.GetAll()
            .Include(x => x.Material)
            .Where(x => x.EnquiryId == enquiryid)
            .ToListAsync();

        if (enquiryMaterials == null)
        {
            throw new UserFriendlyException("Could not found the Enquiry materials!");
        }
        return enquiryMaterials;
    }

    public async Task<EnquiryMaterial> GetAsync(int id)
    {
        var enquiryMaterial = await _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

        if (enquiryMaterial == null)
        {
            throw new UserFriendlyException("Could not found the EnquiryMaterial!");
        }
        return enquiryMaterial;
    }


    public async Task DeleteEnquiryMaterialsAsync(int enquiryMaterialId)
    {
        var enquiryMaterial = await _repository.GetAll().Where(x => x.Id == enquiryMaterialId).FirstOrDefaultAsync();

        if (enquiryMaterial != null)
        {
            enquiryMaterial.IsDeleted = true;
            enquiryMaterial.DeletionTime = DateTime.Now;

            await _repository.UpdateAsync(enquiryMaterial);
        }
    }

    public async Task DeleteEnquiryMaterialByEnquiryIdsAsync(long enquiryId)
    {
        var enquiryMaterials = await _repository.GetAll().Where(x => x.EnquiryId == enquiryId).ToListAsync();

        if (enquiryMaterials != null)
        {
            foreach (var item in enquiryMaterials)
            {
                _repository.Delete(item.Id);
            }
        }
    }

}
