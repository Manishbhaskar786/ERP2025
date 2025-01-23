using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERPack.Customers;
using System.ComponentModel.DataAnnotations;
using ERPack.Enquiries;

namespace ERPack.Estimates
{
    public class Estimate : FullAuditedEntity<long>, IMayHaveTenant
    {
        public long DesignId { get; set; }
        public long EnquiryId { get; set; }
        public string EstimateId { get; set; }
        public string Description { get; set; }
        public long CustomerId { get; set; }
        [Precision(18, 2)]
        public decimal CGSTAmount { get; set; }
        [Precision(18, 2)]
        public decimal IGSTAmount { get; set; }
        [Precision(18, 2)]
        public decimal SGSTAmount { get; set; }
        [Precision(18, 2)]
        public decimal GrossAmount { get; set; }
        [Precision(18, 2)]
        public decimal? TotalAmount { get; set; }
        public string Image { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public virtual int? TenantId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Enquiry Enquiry { get; set; }
        public bool? IsEstimateApproved { get; set; }
    }
}
