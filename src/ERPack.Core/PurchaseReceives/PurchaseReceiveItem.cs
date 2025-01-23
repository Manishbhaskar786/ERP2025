using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ERPack.Materials.Units;
using ERPack.Materials;
using ERPack.PurchaseOrders;
using ERPack.Stores;

namespace ERPack.PurchaseRecieves
{
    public class PurchaseReceiveItem : FullAuditedEntity<int>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        public int? PurchaseReceiveId { get; set; }
        public int? PurchaseOrderItemId { get; set; }
        public int QuantityReceived { get; set; }
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        public virtual PurchaseOrderItem PurchaseOrderItem { get; set; }
    }
}
