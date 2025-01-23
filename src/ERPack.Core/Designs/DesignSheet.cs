using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Designs
{
    public class DesignSheet : FullAuditedEntity<int>
    {
        [StringLength(200)]
        public string DesignName { get; set; }
        [StringLength(50)]
        public string DesignNumber { get; set; }
    }
}
