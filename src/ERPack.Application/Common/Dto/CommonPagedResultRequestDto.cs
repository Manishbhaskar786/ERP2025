using Abp.Application.Services.Dto;
using System;

namespace ERPack.Common.Dto
{
    //custom PagedResultRequestDto
    public class CommonPagedResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
    }
}
