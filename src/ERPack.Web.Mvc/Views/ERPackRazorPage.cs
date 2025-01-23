using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ERPack.Web.Views
{
    public abstract class ERPackRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ERPackRazorPage()
        {
            LocalizationSourceName = ERPackConsts.LocalizationSourceName;
        }
    }
}
