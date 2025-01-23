using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Estimates.Dto
{
    [AutoMap(typeof(Estimate))]
    public class EstimateDto : EntityDto<int>
    {
        public long Id { get; set; }
        public string EstimateId { get; set; }
        public long DesignId { get; set; }
        public long EnquiryId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        [Precision(18, 2)]
        public decimal CGSTAmount { get; set; }
        [Precision(18, 2)]
        public decimal IGSTAmount { get; set; }
        [Precision(18, 2)]
        public decimal SGSTAmount { get; set; }
        [Precision(18, 2)]
        public decimal GrossAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }
        public string Status { get; set; }
        public int? TenantId { get; set; }
        public bool? IsEstimateApproved { get; set; }

    }
}
