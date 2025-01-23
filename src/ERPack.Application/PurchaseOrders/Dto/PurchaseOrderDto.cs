using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace ERPack.PurchaseOrders.Dto
{
    [AutoMap(typeof(PurchaseOrder))]
    public class PurchaseOrderDto : EntityDto<int>
    {
        public virtual int? TenantId { get; set; }
        public int PurchaseItem { get; set; }
        public string PurchaseOrder { get; set; }
        public string POCode { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
