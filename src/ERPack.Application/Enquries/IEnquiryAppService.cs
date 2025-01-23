using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERPack.Common.Dto;
using ERPack.Enquiries.Dto;
using ERPack.Enquries.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Enquiries
{
    public interface IEnquiryAppService : IApplicationService
    {
        Task<long> CreateAsync(EnquiryDto input);
        Task<EnquiryDto> UpdateAsync(EnquiryDto input);
        Task<EnquiryDto> UpdateStatusAsync(long enquiryId, string status);
        Task<EnquiryDto> GetAsync(long enquiryId);
        Task<PagedResultDto<EnquiryDto>> GetAllAsync(CommonPagedResultRequestDto input);
        Task<PagedResultDto<EnquiryDto>> GetAllDesignReadyAsync(CommonPagedResultRequestDto input);
        Task<List<EnquiryMaterialDto>> GetEnquiryMaterialsAsync(int enquiryid);        //Task UpdateEnquiryMaterialsAsync(EnquiryMaterialDto input);
        Task DeleteDesignMaterialAsync(int enquiryMaterialId);
    }
}
