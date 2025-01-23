using ERPack.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Common
{
    public interface IStateMasterAppService
    {
        Task<List<StateMasterDto>> GetStatesAsync(int CountryId=0);
    }
}
