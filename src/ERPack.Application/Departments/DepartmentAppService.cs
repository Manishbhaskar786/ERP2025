using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using Abp.UI;
using ERPack.Authorization;
using ERPack.Departments.Dto;
using ERPack.Users.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Departments
{
    [AbpAuthorize]
    public class DepartmentAppService : ERPackAppServiceBase , IDepartmentAppService
    {
        private readonly DepartmentManager _departmentManager;
        private readonly IObjectMapper _objectMapper;
        readonly IRepository<Department, int> _departmentRepository;

        public DepartmentAppService(IRepository<Department> repository,
            DepartmentManager departmentManager,
            IObjectMapper objectMapper)
        {
            _departmentManager = departmentManager;
            _objectMapper = objectMapper;
            _departmentRepository = repository;
        }

        public  async Task<int> CreateDepartmentAsync(DepartmentDto input)
        {

            var department = ObjectMapper.Map<Department>(input);

            int departmentId =  await _departmentManager.CreateAsync(department);

            return departmentId;
        }

        public async Task<List<DepartmentOutput>> GetDepartmentsAsync()
        {
            var departments = await _departmentManager.GetAllAsync();

            var result = _objectMapper.Map<List<DepartmentOutput>>(departments);

            return result;
        }

        public async Task<DepartmentOutput> GetDepartmentByNameAsync(string name)
        {
            var department = await _departmentManager.GetByNameAsync(name);

            var result = _objectMapper.Map<DepartmentOutput>(department);

            return result;
        }

        public async Task<DepartmentOutput> GetDepartmentByIdAsync(int id)
         {
            var department = await _departmentManager.GetAsync(id);
            var result = _objectMapper.Map<DepartmentOutput>(department);
            return result;
        }

        [AbpAuthorize(PermissionNames.Pages_Department)]
        public Task<PagedResultDto<DepartmentDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var departments = new List<Department>();
            input.MaxResultCount = 99999;
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            departments = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<DepartmentDto>(query.Count(), ObjectMapper.Map<List<DepartmentDto>>(departments));
            return Task.FromResult(result);           
        }
        
        [AbpAuthorize(PermissionNames.Pages_Department)]
        public async Task<DepartmentDto> CreateAsync(DepartmentDto input)
        {
            try
            {
                var existingDepartment = await _departmentRepository.FirstOrDefaultAsync(d => d.DeptName.Trim().ToLower() == input.DeptName.Trim().ToLower());
                if (existingDepartment != null)
                {
                    throw new UserFriendlyException("Department name already exists!");
                }

                var department = ObjectMapper.Map<Department>(input);
                department.TenantId = AbpSession.TenantId;
                await _departmentManager.CreateAsync(department);    
                CurrentUnitOfWork.SaveChanges();
                var departmentResult = ObjectMapper.Map<DepartmentDto>(department);
                return departmentResult;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error Creating department", ex.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Department)]
        [HttpPost]
        public async Task<DepartmentOutput> UpdateAsync(DepartmentOutput input)
        {
            try
            {
                var existingDepartment = await _departmentRepository.FirstOrDefaultAsync(d => d.DeptName.Trim().ToLower() == input.DeptName.Trim().ToLower() && d.Id != input.Id);
                if (existingDepartment != null)
                {
                    throw new UserFriendlyException("Department name already exists!");
                }
                var department = await _departmentManager.GetAsync(input.Id);
                ObjectMapper.Map(input, department);
                await _departmentRepository.UpdateAsync(department);
                return input;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #region Protected Methods
        protected IQueryable<Department> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return _departmentRepository.GetAll().
                WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.DeptName.Contains(input.Keyword) || x.Description.Contains(input.Keyword)).AsQueryable();
        }
        protected IQueryable<Department> ApplySorting(IQueryable<Department> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.Id);
        }
        protected void MapToEntity(DepartmentDto input, Department department)
        {
            ObjectMapper.Map(input, department);
            //department.SetNormalizedNames();
        }
        #endregion
    }
}
