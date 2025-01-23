using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.PurchaseRecieves;
using System;

namespace ERPack.PurchaseReceives.Dto
{
    [AutoMap(typeof(PurchaseReceive))]
    public class PurchaseReceiveDto : EntityDto<int>
    {
        public virtual int? TenantId { get; set; }
        public int PurchaseOrderId { get; set; }
        public string POCode { get; set; }
        public string VendorName { get; set; }
        public DateTime PurchaseReceiveDate { get; set; }
        public int VendorId { get; set; }
    }
}
