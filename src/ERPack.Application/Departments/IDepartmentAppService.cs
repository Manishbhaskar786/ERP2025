using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERPack.Departments.Dto;
using ERPack.Users.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Departments
{
    public interface IDepartmentAppService : IApplicationService
    {
        Task<List<DepartmentOutput>> GetDepartmentsAsync();
        Task<int> CreateDepartmentAsync(DepartmentDto input);
        Task<DepartmentOutput> GetDepartmentByNameAsync(string name);
        Task<DepartmentOutput> GetDepartmentByIdAsync(int id);
        Task<PagedResultDto<DepartmentDto>> GetAllAsync(PagedUserResultRequestDto input);
        Task<DepartmentDto> CreateAsync(DepartmentDto input);
    }
}
