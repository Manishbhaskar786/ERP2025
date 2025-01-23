using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ERPack.Common;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Customers.CustomerTaxationInfos
{
    public class CustomerTaxationInfo : FullAuditedEntity<int>, IMayHaveTenant
    {
        public string AddressName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [StringLength(170)]
        public string City { get; set; }
        public int StateId { get; set; }
        public virtual StateMaster State { get; set; }
        [StringLength(10)]
        public string PinCode { get; set; }
        public int CountryId { get; set; }
        public virtual CountryMaster Country { get; set; }
        [StringLength(20)]
        public string GSTNumber { get; set; }
        [StringLength(20)]
        public string PANNumber { get; set; }
        public bool IsDefault { get; set; }
        public int? TenantId { get; set; }
        public long CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
