using System.Collections.Generic;
using ERPack.Roles.Dto;

namespace ERPack.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
