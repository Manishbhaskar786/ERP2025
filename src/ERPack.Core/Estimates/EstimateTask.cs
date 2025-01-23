using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ERPack.Customers;
using ERPack.Materials;
using ERPack.Materials.Units;

namespace ERPack.Estimates
{
    public class EstimateTask : FullAuditedEntity<long>, IMayHaveTenant
    {
        public long? EstimateId { get; set; }
        public int MaterialId { get; set; }
        public int Qty { get; set; }
        public int UnitId { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Precision(18, 2)]
        public decimal DiscountPercentage { get; set; }
        [Precision(18, 2)]
        public decimal DiscountAmount { get; set; }
        [Precision(18, 2)]
        public decimal CGST { get; set; }
        [Precision(18, 2)]
        public decimal IGST { get; set; }
        [Precision(18, 2)]
        public decimal SGST { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public virtual int? TenantId { get; set; }
        public virtual Estimate Estimate { get; set; }
        public virtual Material Material { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
