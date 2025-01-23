using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Designs
{
    public class ToolConfiguration : FullAuditedEntity<int>
    {
        [StringLength(200)]
        public string ToolConfigurationName { get; set; }
    }
}
