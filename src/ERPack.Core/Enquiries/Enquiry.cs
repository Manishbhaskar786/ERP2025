using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ERPack.Customers;
using ERPack.Designs;
using ERPack.Materials.ItemTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Enquiries
{
    public class Enquiry : FullAuditedEntity<long>, IMayHaveTenant
    {
        public string EnquiryId { get; set; }
        public long CustomerId { get; set; }
        public long DesignUserId { get; set; }
        [StringLength(200)]
        public string DesignName { get; set; }
        [StringLength(50)]
        public string DesignNumber { get; set; }
        [Precision(18, 2)]
        public decimal? BoxLength { get; set; }
        [Precision(18, 2)]
        public decimal? BoxWidth { get; set; }
        [Precision(18, 2)]
        public decimal? BoxHeight { get; set; }
        [Precision(18, 2)]
        public decimal? SheetSizeLength { get; set; }
        [Precision(18, 2)]
        public decimal? SheetSizeWidth { get; set; }
        public int? BoardTypeId { get; set; }
        public virtual BoardType BoardType { get; set; }
        public int ToolTypeId { get; set; }
        public int ToolConfigurationId { get; set; }
        public int ToolFamilyId { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDatetime { get; set; }
        public bool IsHighPriority { get; set; }
        public string DesignImage { get; set; }
        public virtual int? TenantId { get; set; }
        public virtual Customer Customer { get; set; }
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
        public IReadOnlyList<EnquiryMaterial> EnquiryMaterials { get; set; }
    }
}
