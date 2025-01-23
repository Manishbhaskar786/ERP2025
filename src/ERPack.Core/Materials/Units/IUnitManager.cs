using Abp.Domain.Services;
using ERPack.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Materials.Units
{
    public interface IUnitManager : IDomainService
    {
        Task<Unit> GetAsync(int id);
        Task<int> CreateAsync(Unit unit);
        void Cancel(Unit unit);
        Task<List<Unit>> GetAllAsync();
    }
}
