using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPack.Departments;
using ERPack.Materials.ItemTypes;
using ERPack.Materials.Units;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ERPack.Customers;
using ERPack.Estimates;
using ERPack.Enquiries;

namespace ERPack.WorkOrders
{
    public class Workorder : FullAuditedEntity<long>, IMayHaveTenant
    {
        public string WorkorderId { get; set; }
        public virtual int? TenantId { get; set; }
        public string Remarks { get; set; }
        public long? EstimateId { get; set; }
        public string Image { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public bool? IsHighPriority { get; set; }
        public DateTime? TaskIssueDate { get; set; }
        public DateTime? TaskIssueCompleteDate { get; set; }
        public DateTime? TaskIssueActualCompleteDate { get; set; }
        public virtual Estimate Estimate { get; set; }
    }
}
