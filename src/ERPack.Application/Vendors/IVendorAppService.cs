using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERPack.Stores.Dto;
using ERPack.Vendors.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Vendors
{
    public interface IVendorAppService : IApplicationService
    {
        Task<int> CreateAsync(VendorDto input);
        Task<VendorDto> UpdateAsync(VendorDto input);
        Task<VendorDto> GetAsync(int vendorId);
        Task<List<VendorDto>> GetAllVendorsAsync();
    }
}
