using System.Threading.Tasks;
using Abp.Application.Services;
using ERPack.Sessions.Dto;

namespace ERPack.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
