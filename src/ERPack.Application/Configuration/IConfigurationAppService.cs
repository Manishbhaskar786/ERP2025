using System.Threading.Tasks;
using ERPack.Configuration.Dto;

namespace ERPack.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
