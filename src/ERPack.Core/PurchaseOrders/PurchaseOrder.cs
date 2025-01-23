using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPack.Materials.Units;
using ERPack.Vendors;

namespace ERPack.PurchaseOrders
{
    public class PurchaseOrder : FullAuditedEntity<int>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        public int PurchaseItem { get; set; }
        public string DocumentUrl { get; set; }
        public string POCode { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
