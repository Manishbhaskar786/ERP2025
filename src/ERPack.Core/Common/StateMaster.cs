using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPack.Common
{
    public class StateMaster : FullAuditedEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }

        public int CountryId { get; set; }

        [Required]
        [StringLength(50)]
        public string StateName { get; set; }

        [Required]
        [StringLength(10)]
        public string StateCode { get; set; }

        public virtual CountryMaster Country { get; set; }
    }
}
