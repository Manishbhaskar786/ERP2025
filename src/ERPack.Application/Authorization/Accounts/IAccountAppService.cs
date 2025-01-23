using System.Threading.Tasks;
using Abp.Application.Services;
using ERPack.Authorization.Accounts.Dto;

namespace ERPack.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
