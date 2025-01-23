using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace ERPack.Preferences
{
    public class Preference : FullAuditedEntity<int>, IMayHaveTenant
    {
        public string FrontStyle { get; set; }
        public string FrontSize { get; set; }
        public string IdType { get; set; }
        public string NameIdentifier { get; set; }
        public string FixedName { get; set; }
        public string MonthSelection { get; set; }
        public string YearSelection { get; set; }
        public string DisplayId { get; set; }
        public virtual int? TenantId { get; set; }
    }
}
