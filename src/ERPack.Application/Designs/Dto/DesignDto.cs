using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Enquiries.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ERPack.Designs.Dto
{
    [AutoMap(typeof(Design), typeof(BoardType))]
    public class DesignDto : EntityDto<long>
    {
        public string DesignId { get; set; }
        public int CustomerId { get; set; }
        public int DesignUserId { get; set; }
        public string DesignName { get; set; }
        public string DesignNumber { get; set; }
        public string DesignImage { get; set; }
        public string ReportDoc { get; set; }
        public decimal? BoxLength { get; set; }
        public decimal? BoxWidth { get; set; }
        public decimal? BoxHeight { get; set; }
        public decimal? SheetSizeLength { get; set; }
        public decimal? SheetSizeWidth { get; set; }
        public int BoardTypeId { get; set; }
        public int ToolTypeId { get; set; }
        public int ToolConfigurationId { get; set; }
        public int ToolFamilyId { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDatetime { get; set; }
        public DateTime? CompletionDatetime { get; set; }
        public bool IsHighPriority { get; set; }
        public long? TenantId { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public string BoardTypeName { get; set; }
        public long? EnquiryId { get; set; }
        public string? EnquiryNumber { get; set; }
        public IFormFile DesignImageDoc { get; set; }
        public IReadOnlyList<DesignMaterial> DesignMaterials { get; set; }
    }
}
