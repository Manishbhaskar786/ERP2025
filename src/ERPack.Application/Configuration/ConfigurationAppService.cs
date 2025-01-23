using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using ERPack.Configuration.Dto;

namespace ERPack.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ERPackAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
