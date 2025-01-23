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
using Abp.Domain.Repositories;
using Abp.Collections.Extensions;
using Abp.Logging;
using ERPack.Estimates;
using ERPack.Estimates.Dto;
using ERPack.WorkOrders;
using ERPack.Workorders.Dto;
using Microsoft.EntityFrameworkCore;

namespace ERPack.Workorders
{
    [AbpAuthorize]
    public class WorkorderAppService : ERPackAppServiceBase , IWorkorderAppService
    {
        readonly IRepository<Workorder, long> _workorderRepository;
        private readonly WorkorderManager _workorderManager;
        readonly IRepository<WorkorderTask, long> _workorderTaskRepository;

        public WorkorderAppService(IRepository<Workorder, long> workorderRepository,
            WorkorderManager workorderManager,
            IRepository<WorkorderTask, long> workorderTaskRepository)
        {
            _workorderRepository = workorderRepository;
            _workorderManager = workorderManager;
            _workorderTaskRepository = workorderTaskRepository;
        }

        #region Workorder
        public async Task<long> CreateAsync(WorkorderDto input)
        {
            try
            {
                var estimate = ObjectMapper.Map<Workorder>(input);

                long estimateId = await _workorderManager.CreateAsync(estimate);

                return estimateId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }

        }

        public async Task<WorkorderDto> GetAsync(long workorderId)
        {
            var entity = await _workorderManager.GetAsync(workorderId);
            var workorder = ObjectMapper.Map<WorkorderDto>(entity);
            return workorder;
        }

        public Task<PagedResultDto<WorkorderDto>> GetAllAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<Workorder> workorders = query
                .Skip(input.SkipCount)
               // .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<WorkorderDto>(query.Count(), ObjectMapper.Map<List<WorkorderDto>>(workorders));
            return Task.FromResult(result);
        }

        public async Task<List<WorkorderDto>> GetAllWorkordersAsync()
        {
            var estimates = await _workorderManager.GetAllAsync();

            var result = ObjectMapper.Map<List<WorkorderDto>>(estimates);

            return result;
        }

        public async Task DeleteAsync(EntityDto<long> input)
        {
            try
            {
                var estimate = await _workorderManager.GetAsync(input.Id);
                _workorderManager.Cancel(estimate);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }

        }

        #endregion

        #region Workorder Tasks

        public async Task<long> CreateWorkorderTaskAsync(WorkorderTaskDto input)
        {
            try
            {
                var workorderTask = ObjectMapper.Map<WorkorderTask>(input);

                long workorderTaskId = await _workorderManager.CreateWorkorderTaskAsync(workorderTask);

                return workorderTaskId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<List<WorkorderTaskDto>> GetWorkorderTasksAsync(long workorderId)
        {
            var workorderTasks = await _workorderManager.GetWorkorderTasksAsync(workorderId);

            var result = ObjectMapper.Map<List<WorkorderTaskDto>>(workorderTasks);

            return result;
        }

        public Task<PagedResultDto<WorkorderTaskDto>> GetWorkorderTasksByUserAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateWorkorderTasksFilteredQuery(input);

            query = ApplySortingToWorkorderTasks(query, input);

            List<WorkorderTask> workorderTasks = query
                .Skip(input.SkipCount)
              //  .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<WorkorderTaskDto>(query.Count(), ObjectMapper.Map<List<WorkorderTaskDto>>(workorderTasks));
            return Task.FromResult(result);
        }

        #endregion

        #region Workorder SubTasks
        public async Task<long> CreateWorkorderSubTaskAsync(WorkorderSubTaskDto input)
        {
            try
            {
                var workorderSubTask = ObjectMapper.Map<WorkorderSubTask>(input);

                long workorderSubTaskId = await _workorderManager.CreateWorkorderSubTaskAsync(workorderSubTask);

                return workorderSubTaskId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<WorkorderSubTaskDto> GetWorkorderTaskByDepartmentAsync(long workorderId, int departmentId)
        {
            var workorderTask = await _workorderManager.GetWorkorderTaskByDepartmentAsync(workorderId, departmentId);

            var result = ObjectMapper.Map<WorkorderSubTaskDto>(workorderTask);

            return result;
        }

        public async Task<List<WorkorderTaskDto>> GetWorkorderTaskAsync(long workorderTaskId)
        {
            try
            {
                var workorderTasks = await _workorderManager.GetWorkorderTaskAsync(workorderTaskId);

                var result = ObjectMapper.Map<List<WorkorderTaskDto>>(workorderTasks);

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error getting Workorder tasks",ex.Message);
            }
        }

        public async Task<List<WorkorderTaskDto>> GetWorkorderTasksByStatusAsync(string status)
        {
            try
            {
                var workorderTasks = await _workorderManager.GetWorkorderTasksByStatusAsync(status);

                var result = ObjectMapper.Map<List<WorkorderTaskDto>>(workorderTasks);

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error getting Workorder tasks", ex.Message);
            }
        }

        #endregion

        #region Private Methods
        protected IQueryable<Workorder> CreateFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _workorderRepository.GetAll().Include(x=> x.Estimate).ThenInclude(x=> x.Enquiry)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.WorkorderId.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Workorder> ApplySorting(IQueryable<Workorder> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }

        protected IQueryable<WorkorderTask> CreateWorkorderTasksFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _workorderTaskRepository.GetAll().Include(x=> x.Workorder).Include(x=> x.Material)
                .Where(x=> x.UserId == AbpSession.UserId.Value && x.Status != ERPackConsts.Completed)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.WorkOrderTaskId.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<WorkorderTask> ApplySortingToWorkorderTasks(IQueryable<WorkorderTask> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }

        #endregion
    }
}
