using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using ERPack.Authorization.Users;

namespace ERPack.Inventory
{
    public class InventoryIssued : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [StringLength(20)]
        public string IssueCode { get; set; }
        public long? InventoryRequestId { get; set; }
        public string MaterialIssueSlipPath { get; set; }
        public bool IsManual { get; set; }
        [ForeignKey("CreatorUserId")]
        public User User { get; set; }
    }
}
