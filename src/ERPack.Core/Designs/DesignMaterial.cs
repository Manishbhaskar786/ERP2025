using Abp.Domain.Entities.Auditing;
using ERPack.Materials;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Designs
{
    public class DesignMaterial : FullAuditedEntity<int>
    {
        public long DesignId { get; set; }
        public int MaterialId { get; set; }
        [Precision(18, 2)]
        public decimal? Quantity { get; set; }
        public virtual Material Material { get; set; }
    }
}
