using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ERPack.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;
using ERPack.Materials;

namespace ERPack.Inventory
{
    public class InventoryRequest : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [StringLength(20)]
        public string InventoryRequestId { get; set; }
        public int MaterialId { get; set; }
        public long RequestFromUserId { get; set; }
        public decimal ReqQty { get; set; }
        public int? DepartmentId { get; set; }
        public string Remark { get; set; }
        public bool? IsReqClose { get; set; }
        public long? WorkorderTaskId { get; set; }
        [ForeignKey("RequestFromUserId")]
        public virtual User User { get; set; }
        public virtual Material Material { get; set; }

    }
}
