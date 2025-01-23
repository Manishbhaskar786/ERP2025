using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Vendors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ERPack.Estimates
{
    public class EstimateManager : IEstimateManager
    {
        private readonly IRepository<Estimate, long> _estimateRepository;
        private readonly IRepository<EstimateTask, long> _estimateTaskRepository;

        public EstimateManager(IRepository<Estimate, long> estimateRepository,
            IRepository<EstimateTask, long> estimateTaskRepository)
        {
            _estimateRepository = estimateRepository;
            _estimateTaskRepository = estimateTaskRepository;
        }

        #region Estimate Methods
        public async Task<long> CreateAsync(Estimate estimate)
        {
            return await _estimateRepository.InsertAndGetIdAsync(estimate);

        }

        public async Task<Estimate> UpdateAsync(Estimate estimate)
        {
            return await _estimateRepository.UpdateAsync(estimate);
        }

        public async Task<Estimate> GetAsync(long id)
        {
            var estimate = await _estimateRepository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (estimate == null)
            {
                throw new UserFriendlyException("Could not found the estimate, maybe it's deleted!");
            }
            return estimate;
        }

        public async Task<Estimate> GetEstimateByEnquiryIdAsync(long enquiryId)
        {
            var estimate = await _estimateRepository.GetAll().AsNoTracking().Where(x => x.EnquiryId == enquiryId).FirstOrDefaultAsync();

            if (estimate == null)
            {
                throw new UserFriendlyException("Could not found the estimate, maybe it's deleted!");
            }
            return estimate;
        }

        public async Task<List<Estimate>> GetAllAsync()
        {
            var estimates = await _estimateRepository.GetAll().ToListAsync();

            if (estimates == null)
            {
                throw new UserFriendlyException("No estimates found, please contact admin!");
            }
            return estimates;
        }

        public async Task<List<Estimate>> GetApprovedEstimatesAsync()
        {
            var estimates = await _estimateRepository.GetAll().Where(x=> x.Status == ERPackConsts.Approved).ToListAsync();

            if (estimates == null)
            {
                throw new UserFriendlyException("No estimates found, please contact admin!");
            }
            return estimates;
        }

        public void Cancel(Estimate estimate)
        {
            _estimateRepository.Delete(estimate);
        }

        #endregion


        #region Estimate Tasks Methods
        public async Task<long> CreateEstimateTaskAsync(EstimateTask estimateTask)
        {
            return await _estimateTaskRepository.InsertAndGetIdAsync(estimateTask);
        }
        public async Task<EstimateTask> UpdateEstimateTaskAsync(EstimateTask estimateTask)
        {
            return await _estimateTaskRepository.UpdateAsync(estimateTask);
        }

        public async Task<EstimateTask> GetEstimateTaskAsync(long id)
        {
            var estimateTask = await _estimateTaskRepository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (estimateTask == null)
            {
                throw new UserFriendlyException("Could not found the estimate task, maybe it's deleted!");
            }
            return estimateTask;
        }

        public async Task<List<EstimateTask>> GetEstimateTasksAsync(long estimateId)
        {
            try
            {
                var estimateTasks = await _estimateTaskRepository.GetAll().Include(x => x.Unit).Include(x => x.Material)
                    .ThenInclude(x=> x.Department)
                    .Where(x => x.EstimateId == estimateId).ToListAsync();

                if (estimateTasks == null)
                {
                    throw new UserFriendlyException("No estimate tasks found, please contact admin!");
                }
                return estimateTasks;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("Error Getting Estimate Tasks",ex.Message);
            }
        }

        #endregion
    }
}
