using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Estimates
{
    public interface IEstimateManager : IDomainService
    {
        Task<long> CreateAsync(Estimate estimate);
        Task<Estimate> UpdateAsync(Estimate estimate);
        Task<Estimate> GetAsync(long id);
        Task<Estimate> GetEstimateByEnquiryIdAsync(long enquiryId);
        Task<List<Estimate>> GetAllAsync();
        void Cancel(Estimate estimate);
        Task<long> CreateEstimateTaskAsync(EstimateTask estimateTask);
        Task<List<EstimateTask>> GetEstimateTasksAsync(long estimateId);
        Task<EstimateTask> UpdateEstimateTaskAsync(EstimateTask estimateTask);
        Task<EstimateTask> GetEstimateTaskAsync(long id);
        Task<List<Estimate>> GetApprovedEstimatesAsync();
    }
}
