using Abp.Application.Services.Dto;

namespace ERPack.Customers.Dto
{
    public class PagedCustomerSubResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public long CustomerId { get; set; }
    }
}
