using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Departments
{
    public interface IDepartmentManager : IDomainService
    {
        Task<Department> GetAsync(int id);
        Task<int> CreateAsync(Department @event);
        void Cancel(Department @event);
        Task<List<Department>> GetAllAsync();
        Task<Department> GetByNameAsync(string name);
    }
}
