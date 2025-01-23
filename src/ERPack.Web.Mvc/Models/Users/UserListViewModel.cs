using System.Collections.Generic;
using ERPack.Departments;
using ERPack.Departments.Dto;
using ERPack.Roles.Dto;

namespace ERPack.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
        public IReadOnlyList<DepartmentOutput> Departments { get; set; }
    }
}
