using Abp.Application.Services;
using ERPack.Designs.Dto;
using ERPack.Estimates.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Estimates
{
    public interface IEstimateAppService : IApplicationService
    {
        Task<long> CreateAsync(EstimateDto input);
        Task<EstimateDto> UpdateAsync(EstimateDto input);
        Task<EstimateDto> UpdateStatusAsync(long estimateId, string status, bool? IsApproved);
        Task<EstimateDto> GetAsync(long estimateId);
        Task<List<EstimateDto>> GetAllEstimatesAsync();
        Task<long> CreateEstimateTaskAsync(EstimateTaskDto input);
        Task<EstimateTaskDto> UpdateEstimateTaskAsync(EstimateTaskDto input);
        Task<List<EstimateTaskDto>> GetEstimateTasksAsync(long estimateId);
        Task<EstimateDto> GetEstimteByEnquiryIdAsync(long enquiryId);
        Task<List<EstimateDto>> GetApprovedEstimatesAsync();
    }
}
