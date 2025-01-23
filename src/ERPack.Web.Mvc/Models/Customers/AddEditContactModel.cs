using Abp.AutoMapper;
using ERPack.Customers.Dto;

namespace ERPack.Web.Models.Customers
{
    [AutoMap(typeof(ContactDto))]
    public class AddEditContactModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        public int? TenantId { get; set; }
        public long CustomerId { get; set; }
    }
}
