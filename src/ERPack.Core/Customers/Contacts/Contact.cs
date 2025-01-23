using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Customers.Contacts
{
    public class Contact : FullAuditedEntity<long>, IMayHaveTenant
    {
        public string FullName { get; set; }

        [StringLength(100)]
        public string Designation { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        [StringLength(15)]
        public string ContactNo { get; set; }
        public int? TenantId { get; set; }

        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
