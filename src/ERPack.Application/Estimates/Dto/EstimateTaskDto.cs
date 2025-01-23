using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace ERPack.Estimates.Dto
{
    [AutoMap(typeof(EstimateTask))]
    public class EstimateTaskDto
    {
        public long? Id { get; set; }
        public long? EstimateId { get; set; }
        public int MaterialId { get; set; }
        public int DepartmentId { get; set; }
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
        public string MaterialName { get; set; }
        public string DepartmentName { get; set; }
        public string ItemCode { get; set; }
        public string SellingUnitName { get; set; }
        public int? TenantId { get; set; }
        public DateTime? CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }
    }
}
