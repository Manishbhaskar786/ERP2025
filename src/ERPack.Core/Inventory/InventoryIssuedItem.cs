using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ERPack.Departments;
using ERPack.Materials.ItemTypes;
using ERPack.Stores;
using ERPack.Materials;
using ERPack.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPack.Inventory
{
    public class InventoryIssuedItem : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int MaterialId { get; set; }
        public int FromStoreId { get; set; }
        public int ToStoreId { get; set; }
        public int ItemTypeId { get; set; }
        [Precision(18, 2)]
        public decimal QtyTransferred { get; set; }
        public int IssuedDepartmentId { get; set; }
        public long PersonIssuedId { get; set; }
        public long InventoryIssueId { get; set; }
        public virtual ItemType ItemType { get; set; }
        [ForeignKey("IssuedDepartmentId")]
        public virtual Department Department { get; set; }
        [ForeignKey("FromStoreId")]
        public virtual Store FromStore { get; set; }
        [ForeignKey("ToStoreId")]
        public virtual Store ToStore { get; set; }
        public virtual Material Material { get; set; }
        [ForeignKey("CreatorUserId")]
        public virtual User User { get; set; }
    }
}
