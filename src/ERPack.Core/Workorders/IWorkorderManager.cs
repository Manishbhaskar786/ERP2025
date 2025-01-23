using Abp.Domain.Services;
using ERPack.WorkOrders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Workorders
{
    public interface IWorkorderManager : IDomainService
    {
        Task<long> CreateAsync(Workorder workorder);
        Task<Workorder> GetAsync(long id);
        Task<List<Workorder>> GetAllAsync();
        void Cancel(Workorder workorder);
        Task<long> CreateWorkorderTaskAsync(WorkorderTask workorderTask);
        Task<WorkorderTask> UpdateWorkorderTaskAsync(WorkorderTask workorderTask);
        Task<List<WorkorderTask>> GetWorkorderTasksAsync(long workorderId);
        Task<List<WorkorderTask>> GetWorkorderTaskAsync(long workorderTaskId);
        Task<List<WorkorderTask>> GetAllWorkorderTasksAsync();
        Task<List<WorkorderTask>> GetWorkorderTasksByStatusAsync(string status);
        Task<long> CreateWorkorderSubTaskAsync(WorkorderSubTask workorderSubTask);
        Task<List<WorkorderSubTask>> GetAllWorkorderSubTasksAsync();
        Task<WorkorderSubTask> GetWorkorderTaskByDepartmentAsync(long workorderId, int departmentId);
    }
}
