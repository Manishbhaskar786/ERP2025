using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ERPack.Materials;
using ERPack.Authorization.Users;
using ERPack.WorkOrders;

namespace ERPack.Workorders
{
    public class WorkorderTask : FullAuditedEntity<long>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        [StringLength(50)]
        public string WorkOrderTaskId { get; set; }
        public long? WorkorderId { get; set; }
        public long? WorkorderSubTaskId { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public int? UnitId { get; set; }
        public long? UserId { get; set; }
        public DateTime? TaskIssueDate { get; set; }
        public DateTime? TaskIssueCompleteDate { get; set; }
        public DateTime? TaskIssueActualCompleteDate { get; set; }
        [StringLength(50)]
        public string Status { get; set; }  
        public virtual Material Material { get; set; }
        public virtual WorkorderSubTask WorkorderSubTask { get; set; }
        public virtual User User { get; set; }
        public virtual Workorder Workorder { get; set; }
    }
}
