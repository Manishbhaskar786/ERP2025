using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using ERPack.Common.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Extensions;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Collections.Extensions;
using Abp.Logging;
using ERPack.WorkOrders;
using ERPack.Workorders.Dto;
using ERPack.Materials.Dto;
using ERPack.Materials;
using ERPack.Production.Dto;
using ERPack.Inventory.Dto;
using ERPack.Inventory;

namespace ERPack.Workorders
{
    [AbpAuthorize]
    public class ProductionAppService : ERPackAppServiceBase, IProductionAppService
    {
        readonly IRepository<Workorder, long> _workorderRepository;
        readonly IRepository<WorkorderTask, long> _workorderTaskRepository;
        private readonly WorkorderManager _workorderManager;
        private readonly InventoryRequestManager _inventoryRequestManager;

        public ProductionAppService(IRepository<Workorder, long> workorderRepository,
            IRepository<WorkorderTask, long> workorderTaskRepository,
            WorkorderManager workorderManager,
            InventoryRequestManager inventoryRequestManager)
        {
            _workorderRepository = workorderRepository;
            _workorderTaskRepository = workorderTaskRepository;
            _workorderManager = workorderManager;
            _inventoryRequestManager = inventoryRequestManager;
        }

        public async Task<List<WorkorderTaskDto>> GetWorkorderTasksAsync(long workorderId)
        {
            var workorderTasks = await _workorderManager.GetWorkorderTasksAsync(workorderId);

            var result = ObjectMapper.Map<List<WorkorderTaskDto>>(workorderTasks);

            return result;
        }

        public async Task<bool> UpdateWorkorderTaskStatusAsync(long workorderTaskId, string status)
        {
            try
            {
                var entity = await _workorderTaskRepository.GetAsync(workorderTaskId);

                entity.Status = status;

                if (status == ERPackConsts.Completed)
                {
                    var workOrderTasks = _workorderTaskRepository.GetAll().Where(r => r.WorkorderId == entity.WorkorderId).ToList();

                    if (workOrderTasks.TrueForAll(x => x.Status == ERPackConsts.Completed))
                    {
                        var workorder = await _workorderManager.GetAsync(entity.WorkorderId.Value);
                        workorder.Status = ERPackConsts.Completed;
                    }
                    var inventoryRequest = await _inventoryRequestManager.GetByTaskId(workorderTaskId);
                    if(inventoryRequest != null)
                    {
                        inventoryRequest.IsReqClose = true;
                    }
                }
                if (status == ERPackConsts.StockReceived)
                {
                    var inventoryRequest = await _inventoryRequestManager.GetByTaskId(workorderTaskId);
                    if (inventoryRequest != null)
                    {
                        inventoryRequest.IsReqClose = true;
                    }
                }

                await _workorderManager.UpdateWorkorderTaskAsync(entity);
                await CurrentUnitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return false;
            }
        }

        protected IQueryable<Workorder> CreateFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _workorderRepository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.WorkorderId.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Workorder> ApplySorting(IQueryable<Workorder> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }

        protected void MapToEntity(WorkorderTaskDto input, WorkorderTask workorderTask)
        {
            ObjectMapper.Map(input, workorderTask);
        }
    }
}
