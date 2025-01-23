using Abp.Application.Services;
using ERPack.Designs.Dto;
using ERPack.Estimates.Dto;
using ERPack.Production.Dto;
using ERPack.Workorders.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Workorders
{
    public interface IProductionAppService : IApplicationService
    {
        Task<bool> UpdateWorkorderTaskStatusAsync(long workorderTaskId, string status);
    }
}
