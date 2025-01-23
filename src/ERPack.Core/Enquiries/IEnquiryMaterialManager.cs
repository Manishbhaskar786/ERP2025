using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Enquiries;

public interface IEnquiryMaterialManager : IDomainService
{
    Task<List<EnquiryMaterial>> GetAllByEnquiryIdAsync(int enquiryid);
    Task DeleteEnquiryMaterialsAsync(int enquiryMaterialId);
    Task DeleteEnquiryMaterialByEnquiryIdsAsync(long enquiryId);

}