using Abp.Application.Services;
using ERPack.Customers.Dto;
using ERPack.MultiTenancy.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.HostTaxationInfoDetails
{
    public interface IHostTaxationInfoAppService : IApplicationService
    {
        Task<HostTaxationDto> GetAsync(int hostTaxationId);
        Task<(int, string)> CreateAsync(HostTaxationDto input);
        Task<(HostTaxationDto, string)> UpdateAsync(HostTaxationDto input);
        Task<List<HostTaxationDto>> GetTaxationDetailsByTenantId(int tenentId);
    }
}
