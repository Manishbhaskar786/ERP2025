using Abp.Authorization;
using ERPack.Authorization.Roles;
using ERPack.Authorization.Users;

namespace ERPack.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
