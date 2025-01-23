using Abp.Application.Services.Dto;
using System;

namespace ERPack.Vendors.Dto
{
    //custom PagedResultRequestDto
    public class PagedVendorResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
    }
}
