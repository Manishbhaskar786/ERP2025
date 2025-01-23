using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using ERPack.PurchaseOrders;
using ERPack.Vendors;

namespace ERPack.PurchaseRecieves
{
    public class PurchaseReceive : FullAuditedEntity<int>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int VendorId { get; set; }
        public DateTime? PurchaseReceiveDate { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
