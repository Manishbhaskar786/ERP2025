using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ERPack.Authorization.Users;
using ERPack.Stores;

namespace ERPack.Materials.MaterialInventories
{
    public class MaterialInventory : FullAuditedEntity<int>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int MaterialId { get; set; }
        public int StoreId { get; set; }
        [Precision(18, 2)]
        public decimal Quantity { get; set; }
    }
}
