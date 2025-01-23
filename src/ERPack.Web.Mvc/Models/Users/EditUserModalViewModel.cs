using System.Collections.Generic;
using System.Linq;
using ERPack.Departments.Dto;
using ERPack.Roles.Dto;
using ERPack.Users.Dto;

namespace ERPack.Web.Models.Users
{
    public class EditUserModalViewModel
    {
        public UserDto User { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
        public IReadOnlyList<DepartmentOutput> Departments { get; set; }

        public bool UserIsInRole(RoleDto role)
        {
            return User.RoleNames != null && User.RoleNames.Any(r => r == role.NormalizedName);
        }
    }
}
