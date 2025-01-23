using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Departments
{
    public class DepartmentManager : IDepartmentManager
    {
        private readonly IRepository<Department, int> _departmentRepository;

        public DepartmentManager(
            IRepository<Department, int> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public void Cancel(Department department)
        {
            _departmentRepository.Delete(department);
        }

        public async Task<int> CreateAsync(Department department)
        {
                return await _departmentRepository.InsertAndGetIdAsync(department);
        }

        public Task<Department> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                var department = _departmentRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();

                if (department == null)
                {
                    throw new UserFriendlyException("Could not found the department, maybe it's deleted!");
                }
                return department;
            });
        }

        public Task<Department> GetByNameAsync(string name)
        {
            return Task.Run(() =>
            {
                var department = _departmentRepository.GetAll().Where(x => x.DeptName == name).FirstOrDefault();

                if (department == null)
                {
                    throw new UserFriendlyException("Could not found the department, maybe it's deleted!");
                }
                return department;
            });
        }

        public async Task<List<Department>> GetAllAsync()
        {
            var departments = await _departmentRepository.GetAll().Where(x => !x.IsDeleted).ToListAsync();

            if (departments == null)
            {
                throw new UserFriendlyException("No departments found, please contact admin!");
            }
            return departments;

        }
    }
}
