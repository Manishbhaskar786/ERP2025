using Abp.AutoMapper;
using ERPack.Departments.Dto;
using ERPack.Users.Dto;

namespace ERPack.Web.Models.Department
{
    [AutoMapFrom(typeof(DepartmentDto))]
    public class AddEditDepartmentModel
    {
        public string DeptName { get; set; }
        public int? TenantId { get; set; }
        public string Description { get; set; }
    }
}
