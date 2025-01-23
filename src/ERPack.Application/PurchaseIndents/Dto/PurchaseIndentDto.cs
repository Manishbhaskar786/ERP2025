using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace ERPack.PurchaseIndents.Dto
{
    [AutoMap(typeof(PurchaseIndent))]
    public class PurchaseIndentDto : EntityDto<int>
    {
        public virtual int? TenantId { get; set; }
        public int? ItemTypeId { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string ItemCode { get; set; }
        public int? Quantity { get; set; }
        public DateTime? RequiredBy { get; set; }
        public long? RequestedBy { get; set; }
        public string RequestedByUser { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string Remark { get; set; }
    }
}
