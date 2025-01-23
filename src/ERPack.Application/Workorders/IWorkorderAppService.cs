using Abp.Application.Services;
using ERPack.Designs.Dto;
using ERPack.Estimates.Dto;
using ERPack.Workorders.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Workorders
{
    public interface IWorkorderAppService : IApplicationService
    {
        Task<long> CreateAsync(WorkorderDto input);
        Task<WorkorderDto> GetAsync(long workorderId);
        Task<long> CreateWorkorderTaskAsync(WorkorderTaskDto input);
        Task<List<WorkorderTaskDto>> GetWorkorderTaskAsync(long workorderTaskId);
        Task<List<WorkorderTaskDto>> GetWorkorderTasksAsync(long estimateId);
        Task<List<WorkorderTaskDto>> GetWorkorderTasksByStatusAsync(string status);
        Task<long> CreateWorkorderSubTaskAsync(WorkorderSubTaskDto input);
        Task<WorkorderSubTaskDto> GetWorkorderTaskByDepartmentAsync(long workorderId, int departmentId);
    }
}
