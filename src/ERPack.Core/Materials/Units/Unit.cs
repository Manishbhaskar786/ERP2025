using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Materials.Units
{
    public class Unit : FullAuditedEntity<int>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string UnitName { get; set; }
    }
}
