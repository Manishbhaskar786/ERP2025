using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using ERPack.Customers.Contacts;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Customers.Dto
{
    [AutoMap(typeof(Contact))]
    public class ContactDto : EntityDto<long>
    {
        [Required]
        public string FullName { get; set; }

        [StringLength(100)]
        public string Designation { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        [StringLength(15)]
        [Required]
        public string ContactNo { get; set; }
        public int? TenantId { get; set; }

        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
