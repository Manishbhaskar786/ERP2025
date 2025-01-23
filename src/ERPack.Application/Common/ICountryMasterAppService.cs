using Abp.Application.Services.Dto;
using ERPack.Common.Dto;
using ERPack.Customers.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Common
{
    public interface ICountryMasterAppService
    {
        Task<List<CountryMasterDto>> GetCountriesAsync(int CountryId=0);
    }
}
