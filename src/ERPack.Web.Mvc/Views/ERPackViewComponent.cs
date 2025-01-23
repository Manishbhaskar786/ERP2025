using Abp.AspNetCore.Mvc.ViewComponents;

namespace ERPack.Web.Views
{
    public abstract class ERPackViewComponent : AbpViewComponent
    {
        protected ERPackViewComponent()
        {
            LocalizationSourceName = ERPackConsts.LocalizationSourceName;
        }
    }
}
