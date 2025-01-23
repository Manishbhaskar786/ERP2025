using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ERPack.HostTaxation
{
    public class HostTaxationInfo : FullAuditedEntity<int>, IMayHaveTenant
    {
        public string AddressName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [StringLength(170)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(10)]
        public string PinCode { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        [StringLength(20)]
        public string GSTNumber { get; set; }
        [StringLength(20)]
        public string PANNumber { get; set; }
        public bool IsDefault { get; set; }
        public virtual int? TenantId { get; set; }
    }
}
