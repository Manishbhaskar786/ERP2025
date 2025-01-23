using Abp.Application.Services.Dto;
using System;

namespace ERPack.Users.Dto
{
    //custom PagedResultRequestDto
    public class PagedCustomerResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
    }
}
