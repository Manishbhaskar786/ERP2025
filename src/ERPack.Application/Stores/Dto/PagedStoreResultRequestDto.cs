using Abp.Application.Services.Dto;
using System;

namespace ERPack.Vendors.Dto
{
    //custom PagedResultRequestDto
    public class PagedStoreResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
    }
}
