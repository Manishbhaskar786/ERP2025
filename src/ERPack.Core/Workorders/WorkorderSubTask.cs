using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ERPack.Materials;
using ERPack.Departments;

namespace ERPack.Workorders
{
    public class WorkorderSubTask : FullAuditedEntity<long>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        [StringLength(50)]
        public string WorkorderSubTaskId { get; set; }
        public long? WorkorderId { get; set; }
        public int DepartmentId { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public virtual Department Department { get; set; }
    }
}
