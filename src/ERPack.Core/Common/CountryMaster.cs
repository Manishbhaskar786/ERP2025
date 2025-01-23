using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPack.Common
{
    public class CountryMaster : FullAuditedEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }

        [Required]
        [StringLength(250)]
        public string CountryName { get; set; }

        [Required]
        [StringLength(10)]
        public string CountryCode { get; set; }
    }
}
