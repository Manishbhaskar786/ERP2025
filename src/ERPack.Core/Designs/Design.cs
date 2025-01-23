using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ERPack.Enquiries;
using System;
using System.Collections.Generic;

namespace ERPack.Designs
{
    public class Design : FullAuditedEntity<long>, IMayHaveTenant
    {
        public long EnquiryId { get; set; }
        public string DesignId { get; set; }
        public DateTime? CompletionDatetime { get; set; }
        public string Status { get; set; }
        public virtual int? TenantId { get; set; }
        public string ReportDoc { get; set; }
        public Enquiry Enquiry { get; set; }

        public IReadOnlyList<DesignMaterial> DesignMaterials { get; set; }
    }
}
