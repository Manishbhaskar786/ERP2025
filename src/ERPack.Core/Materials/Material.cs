using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPack.Departments;
using ERPack.Materials.Units;
using ERPack.Materials.ItemTypes;
using Microsoft.EntityFrameworkCore;

namespace ERPack.Materials
{
    public class Material : FullAuditedEntity<int>, IMayHaveTenant
    {
        public int ItemTypeId { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public int BuyingUnitId { get; set; }
        public int SellingUnitId { get; set; }
        public int MaterialTypeId { get; set; }
        public int? CategoryId { get; set; }
        public string ItemCode { get; set; }
        public string HSN { get; set; }
        [Precision(18, 2)]
        public decimal? CGST { get; set; }
        [Precision(18, 2)]
        public decimal? SGST { get; set; }
        [Precision(18, 2)]
        public decimal? IGST { get; set; }
        [Precision(18, 2)]
        public decimal? Qty { get; set; }
        [Precision(18, 2)]
        public decimal? BuyingPrice { get; set; }
        [Precision(18, 2)]
        public decimal? SellingPrice { get; set; }
        public string MaterialName { get; set; }
        public int? DepartmentId { get; set; }
        public int? TenantId { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual Department Department { get; set; }
        public virtual Unit SellingUnit { get; set; }
        public virtual Unit BuyingUnit { get; set; }
    }
}
