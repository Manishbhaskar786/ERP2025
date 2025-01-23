using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.PurchaseOrders.Dto
{
    [AutoMap(typeof(PurchaseOrderItem))]
    public class PurchaseOrderItemDto : EntityDto<int>
    {
        public virtual int? TenantId { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? MaterialId { get; set; }
        public int? ItemTypeId { get; set; }
        public int? UnitId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? SGST { get; set; }
        public decimal? GSTAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string ItemType { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string BuyingUnit { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
    }
}
