using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERPack.Authorization.Users;
using ERPack.Departments;
using ERPack.Materials.ItemTypes;
using ERPack.Materials;
using ERPack.Stores;
using System.ComponentModel.DataAnnotations.Schema;
using ERPack.Materials.Units;

namespace ERPack.PurchaseOrders
{
    public class PurchaseOrderItem : FullAuditedEntity<int>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int MaterialId { get; set; }
        public int? ItemTypeId { get; set; }
        public int? UnitId { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        [Precision(18, 2)]
        public decimal? Amount { get; set; }
        [Precision(18, 2)]
        public decimal? CGST { get; set; }
        [Precision(18, 2)]
        public decimal? IGST { get; set; }
        [Precision(18, 2)]
        public decimal? SGST { get; set; }
        [Precision(18, 2)]
        public decimal? GSTAmount { get; set; }
        [Precision(18, 2)]
        public decimal? TotalAmount { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual Material Material { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
    }
}
