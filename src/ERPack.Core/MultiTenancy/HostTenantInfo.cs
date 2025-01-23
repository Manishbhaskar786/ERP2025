using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ERPack.MultiTenancy
{
    public class HostTenantInfo: FullAuditedEntity<int>
    {
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
        public string Logo { get; set; }
        [StringLength(100)]
        public string BankName { get; set; }
        [StringLength(100)]
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        [StringLength(50)]
        public string IFSCCode { get; set; }
        [StringLength(100)]
        public string GSTNumber { get; set; }
        [StringLength(100)]
        public string PANNumber { get; set; }

        public HostTenantInfo()
        {            
        }
    }
}
