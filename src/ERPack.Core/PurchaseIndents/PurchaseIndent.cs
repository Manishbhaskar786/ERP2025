using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ERPack.Departments;
using ERPack.Materials.ItemTypes;
using ERPack.Materials;
using ERPack.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPack.PurchaseIndents
{
    public class PurchaseIndent : FullAuditedEntity<long>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        public int? ItemTypeId { get; set; }
        public int MaterialId { get; set; }
        [Precision(18, 2)]
        public int? Quantity { get; set; }
        public DateTime? RequiredBy { get; set; }
        public long? RequestedBy { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string Remark { get; set; }
        public virtual Material Material { get; set; }
        public virtual ItemType ItemType { get; set; }
        [ForeignKey("RequestedBy")]
        public virtual User User { get; set; }
    }
}
