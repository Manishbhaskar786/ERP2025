using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.WorkOrders;
using Microsoft.EntityFrameworkCore;
using System;

namespace ERPack.Workorders.Dto
{
    [AutoMap(typeof(Workorder))]
    public class WorkorderDto : EntityDto<int>
    {
        public int Id { get; set; }
        public string WorkorderId { get; set; }
        public long EstimateId { get; set; }
        public string EstimateCode { get; set; }
        public string EnquiryCode { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool? IsHighPriority { get; set; }
        public DateTime? TaskIssueDate { get; set; }
        public DateTime? TaskIssueCompleteDate { get; set; }
        public DateTime? TaskIssueActualCompleteDate { get; set; }
        public DateTime CreationTime { get; set; }
        public int? TenantId { get; set; }

    }
}
