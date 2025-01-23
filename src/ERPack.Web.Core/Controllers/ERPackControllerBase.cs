using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;

namespace ERPack.Controllers
{
    public abstract class ERPackControllerBase: AbpController
    {
        protected ERPackControllerBase()
        {
            LocalizationSourceName = ERPackConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
