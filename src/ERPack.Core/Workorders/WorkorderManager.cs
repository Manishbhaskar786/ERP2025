using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Materials;
using ERPack.WorkOrders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ERPack.Workorders
{
    public class WorkorderManager : IWorkorderManager
    {
        private readonly IRepository<Workorder, long> _workorderRepository;
        private readonly IRepository<WorkorderTask, long> _workorderTaskRepository;
        private readonly IRepository<WorkorderSubTask, long> _workorderSubTaskRepository;

        public WorkorderManager(IRepository<Workorder, long> workorderRepository,
            IRepository<WorkorderTask, long> workorderTaskRepository,
            IRepository<WorkorderSubTask, long> workorderSubTaskRepository)
        {
            _workorderRepository = workorderRepository;
            _workorderTaskRepository = workorderTaskRepository;
            _workorderSubTaskRepository = workorderSubTaskRepository;
        }

        #region Workorder
        public async Task<long> CreateAsync(Workorder workorder)
        {
            workorder.Estimate = null;
            return await _workorderRepository.InsertOrUpdateAndGetIdAsync(workorder);
        }

        public async Task<Workorder> GetAsync(long id)
        {
            var workorder = await _workorderRepository.GetAll().Include(x=> x.Estimate).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (workorder == null)
            {
                throw new UserFriendlyException("Could not found the workorder, maybe it's deleted!");
            }
            return workorder;

        }

        public async Task<List<Workorder>> GetAllAsync()
        {
            var workorders = await _workorderRepository.GetAll().ToListAsync();

            if (workorders == null)
            {
                throw new UserFriendlyException("No workorders found, please contact admin!");
            }
            return workorders;
        }

        public void Cancel(Workorder workorder)
        {
            _workorderRepository.Delete(workorder);
        }

        #endregion

        #region Workorder Tasks

        public async Task<long> CreateWorkorderTaskAsync(WorkorderTask workorderTask)
        {
            return await _workorderTaskRepository.InsertAndGetIdAsync(workorderTask);

        }

        public async Task<WorkorderTask> UpdateWorkorderTaskAsync(WorkorderTask workorderTask)
        {
            return await _workorderTaskRepository.UpdateAsync(workorderTask);
        }

        public async Task<List<WorkorderTask>> GetAllWorkorderTasksAsync()
        {
            try
            {
                var workorderTasks = await _workorderTaskRepository.GetAll().ToListAsync();

                if (workorderTasks == null)
                {
                    throw new UserFriendlyException("No workorder tasks found, please contact admin!");
                }
                return workorderTasks;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error Getting Workorder Tasks", ex.Message);
            }
        }

        public async Task<List<WorkorderTask>> GetWorkorderTasksByStatusAsync(string status)
        {
            try
            {
                var workorderTasks = await _workorderTaskRepository.GetAll()
                    .Where(x=> x.Status == status).ToListAsync();

                if (workorderTasks == null)
                {
                    throw new UserFriendlyException("No workorder tasks found, please contact admin!");
                }
                return workorderTasks;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error Getting Workorder Tasks", ex.Message);
            }
        }

        public async Task<List<WorkorderTask>> GetWorkorderTasksAsync(long workorderId)
        {
            try
            {
                var workorderTasks = await _workorderTaskRepository.GetAll()
                    .Include(x => x.WorkorderSubTask)
                    .Include(x=> x.User)
                    .Include(x => x.Material).ThenInclude(x=> x.Department)
                    .Where(x => x.WorkorderId == workorderId && x.WorkorderSubTask.WorkorderId == workorderId)
                    .GroupBy(x => x.WorkorderSubTaskId).Select(g => g.FirstOrDefault()).ToListAsync();

                if (workorderTasks == null)
                {
                    throw new UserFriendlyException("No workorder tasks found, please contact admin!");
                }
                return workorderTasks;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("Error Getting Workorder Tasks",ex.Message);
            }
        }

        public async Task<List<WorkorderTask>> GetWorkorderTaskAsync(long workorderTaskId)
        {
            try
            {
                var workorderTasks = await _workorderTaskRepository.GetAll()
                    .Include(x => x.Material)
                    .Where(x => x.Id == workorderTaskId)
                    .ToListAsync();

                if (workorderTasks == null)
                {
                    throw new UserFriendlyException("No workorder tasks found, please contact admin!");
                }
                return workorderTasks;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error Getting Workorder Tasks", ex.Message);
            }
        }

        #endregion

        #region Workorder Subtasks

        public async Task<long> CreateWorkorderSubTaskAsync(WorkorderSubTask workorderSubTask)
        {
            return await _workorderSubTaskRepository.InsertAndGetIdAsync(workorderSubTask);
        }

        public async Task<List<WorkorderSubTask>> GetAllWorkorderSubTasksAsync()
        {
            try
            {
                var workorderSubTasks = await _workorderSubTaskRepository.GetAll().ToListAsync();

                if (workorderSubTasks == null)
                {
                    throw new UserFriendlyException("No workorder sub tasks found, please contact admin!");
                }
                return workorderSubTasks;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error Getting Workorder Sub Tasks", ex.Message);
            }
        }

        public async Task<WorkorderSubTask> GetWorkorderTaskByDepartmentAsync(long workorderId, int departmentId)
        {
            try
            {
                var workorderSubTask = await _workorderSubTaskRepository.GetAll().AsNoTracking().Where(x=> x.WorkorderId == workorderId
                 && x.DepartmentId == departmentId).FirstOrDefaultAsync();

                if (workorderSubTask == null)
                {
                    throw new UserFriendlyException("No workorder task found, please contact admin!");
                }
                return workorderSubTask;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error Getting Workorder Sub Task", ex.Message);
            }
        }

        #endregion
    }
}
