using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Designs
{
    public class BoardType : FullAuditedEntity<int>
    {
        [StringLength(200)]
        public string BoardTypeName { get; set; }
    }
}
