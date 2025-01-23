using System.Collections.Generic;
using ERPack.Roles.Dto;

namespace ERPack.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}