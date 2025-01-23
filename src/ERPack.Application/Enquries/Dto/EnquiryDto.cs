using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Designs.Dto;
using ERPack.Enquries.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ERPack.Enquiries.Dto
{
    [AutoMap(typeof(Enquiry))]
    public class EnquiryDto : EntityDto<long>
    {
        public string EnquiryId { get; set; }
        public int CustomerId { get; set; }
        public long DesignUserId { get; set; }
        public string DesignName { get; set; }
        public string DesignNumber { get; set; }
        public decimal? BoxLength { get; set; }
        public decimal? BoxWidth { get; set; }
        public decimal? BoxHeight { get; set; }
        public decimal? SheetSizeLength { get; set; }
        public decimal? SheetSizeWidth { get; set; }
        public int? BoardTypeId { get; set; }
        public string BoardTypeName { get; set; }
        public int ToolTypeId { get; set; }
        public int ToolConfigurationId { get; set; }
        public int ToolFamilyId { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDatetime { get; set; }
        public bool IsHighPriority { get; set; }
        public string DesignImage { get; set; }
        public IFormFile DesignImageDoc { get; set; }
        public long? TenantId { get; set; }
        public DateTime? CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }
        public string CustomerName { get; set; }
        public bool? IsEstimateApproved { get; set; }
        public bool IsBraile { get; set; }
        public decimal? BraileLength { get; set; }
        [Precision(18, 2)]
        public decimal? BraileWidth { get; set; }
        [Precision(18, 2)]
        public string BraileComments { get; set; }
        public bool IsEmboss { get; set; }
        public decimal? EmbossLength { get; set; }
        [Precision(18, 2)]
        public decimal? EmbossWidth { get; set; }
        [Precision(18, 2)]
        public string EmbossComments { get; set; }
        public int? NumberOfUps { get; set; }
        public IReadOnlyList<EnquiryMaterialDto> EnquiryMaterials { get; set; }
    }
}
