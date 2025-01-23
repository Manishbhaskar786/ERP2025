using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using ERPack.Common.Dto;
using ERPack.Enquiries.Dto;
using ERPack.Enquiries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Extensions;
using System.Linq;
using ERPack.Designs.Dto;
using Abp.Domain.Repositories;
using Abp.Collections.Extensions;
using Abp.Logging;
using ERPack.Designs.DesignMaterials;
using ERPack.Estimates;
using ERPack.Estimates.Dto;
using ERPack.Vendors.Dto;
using ERPack.Vendors;

namespace ERPack.Estimates
{
    [AbpAuthorize]
    public class EstimateAppService : ERPackAppServiceBase , IEstimateAppService
    {
        readonly IRepository<Estimate, long> _estimateRepository;
        readonly IRepository<Enquiry, long> _enquiryRepository;
        private readonly EstimateManager _estimateManager;
        private readonly EnquiryManager _enquiryManager;

        public EstimateAppService(IRepository<Estimate, long> estimateRepository,
            IRepository<Enquiry, long> enquiryRepository,
            EnquiryManager enquiryManager,
            EstimateManager estimateManager)
        {
            _estimateRepository = estimateRepository;
            _estimateManager = estimateManager;
            _enquiryRepository = enquiryRepository;
            _enquiryManager = enquiryManager;
        }

        #region

        public async Task<long> CreateAsync(EstimateDto input)
        {
            try
            {
                var estimate = ObjectMapper.Map<Estimate>(input);

                long estimateId = await _estimateManager.CreateAsync(estimate);

                return estimateId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }

        }

        public async Task<EstimateDto> UpdateAsync(EstimateDto input)
        {
            try
            {
                var entity = await _estimateRepository.GetAsync(input.Id);

                input.CreationTime = entity.CreationTime;
                input.CreatorUserId = entity.CreatorUserId;
                input.Status = entity.Status;
                input.IsEstimateApproved = entity.IsEstimateApproved;
                MapToEntity(input, entity);
                var result = await _estimateManager.UpdateAsync(entity);

                await CurrentUnitOfWork.SaveChangesAsync();

                var estimate = ObjectMapper.Map<EstimateDto>(result);

                return estimate;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return null;
            }
        }

        public async Task<EstimateDto> UpdateStatusAsync(long estimateId, string status ,bool? IsApproved)
        {
            try
            {
                var entity = await _estimateRepository.GetAsync(estimateId);

                entity.Status = status;
                entity.IsEstimateApproved= IsApproved;

                var result = await _estimateManager.UpdateAsync(entity);

                await CurrentUnitOfWork.SaveChangesAsync();

                var estimate = ObjectMapper.Map<EstimateDto>(result);

                return estimate;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return null;
            }
        }

        public async Task<EstimateDto> GetAsync(long estimateId)
        {
            var entity = await _estimateManager.GetAsync(estimateId);
            var estimate = ObjectMapper.Map<EstimateDto>(entity);
            return estimate;
        }
        public async Task DeleteAsync(EntityDto<long> input)
        {
            try
            {
                var estimate = await _estimateManager.GetAsync(input.Id);
                _estimateManager.Cancel(estimate);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }

        }

        public async Task<EstimateDto> GetEstimteByEnquiryIdAsync(long enquiryId)
        {
            var entity = await _estimateManager.GetEstimateByEnquiryIdAsync(enquiryId);
            var estimate = ObjectMapper.Map<EstimateDto>(entity);
            return estimate;
        }

        public Task<PagedResultDto<EstimateDto>> GetAllAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            
            query = ApplySorting(query, input);

            List<Estimate> estimates = query
                .Skip(input.SkipCount)
            //    .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<EstimateDto>(query.Count(), ObjectMapper.Map<List<EstimateDto>>(estimates));
            return Task.FromResult(result);
        }

        public async Task<List<EstimateDto>> GetAllEstimatesAsync()
        {
            var estimates = await _estimateManager.GetAllAsync();

            var result = ObjectMapper.Map<List<EstimateDto>>(estimates);

            return result;
        }

        public async Task<List<EstimateDto>> GetApprovedEstimatesAsync()
        {
            var estimates = await _estimateManager.GetApprovedEstimatesAsync();

            var result = ObjectMapper.Map<List<EstimateDto>>(estimates);

            return result;
        }

        #endregion

        #region Estimate Tasks

        public async Task<long> CreateEstimateTaskAsync(EstimateTaskDto input)
        {
            try
            {
                var estimateTask = ObjectMapper.Map<EstimateTask>(input);

                long estimateTaskId = await _estimateManager.CreateEstimateTaskAsync(estimateTask);

                return estimateTaskId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<EstimateTaskDto> UpdateEstimateTaskAsync(EstimateTaskDto input)
        {
            try
            {
                var entity = await _estimateManager.GetEstimateTaskAsync(input.Id.Value);

                input.CreationTime = entity.CreationTime;
                input.CreatorUserId = entity.CreatorUserId;
                input.EstimateId = entity.EstimateId;

                MapToEstimateTaskEntity(input, entity);

                var result = await _estimateManager.UpdateEstimateTaskAsync(entity);

                await CurrentUnitOfWork.SaveChangesAsync();

                var estimateTask = ObjectMapper.Map<EstimateTaskDto>(result);

                return estimateTask;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return null;
            }
        }

        public async Task<List<EstimateTaskDto>> GetEstimateTasksAsync(long estimateId)
        {
            var estimateTasks = await _estimateManager.GetEstimateTasksAsync(estimateId);

            var result = ObjectMapper.Map<List<EstimateTaskDto>>(estimateTasks);

            return result;
        }

        #endregion

        protected IQueryable<Estimate> CreateFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _estimateRepository.GetAllIncluding(x => x.Customer)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.EstimateId.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Estimate> ApplySorting(IQueryable<Estimate> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }

        protected void MapToEntity(EstimateDto input, Estimate estimate)
        {
            ObjectMapper.Map(input, estimate);
        }

        protected void MapToEstimateTaskEntity(EstimateTaskDto input, EstimateTask estimateTask)
        {
            ObjectMapper.Map(input, estimateTask);
        }
    }
}
