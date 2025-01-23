using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPack.Currency
{
    public class CurrencyMaster : FullAuditedEntity<int>
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary Key
        [Required]
        [StringLength(20)]
        public string Name { get; set; } // Currency Name (e.g., US Dollar)
        [Required]
        [StringLength(20)]
        public string Code { get; set; } // Currency Code (e.g., USD)
        [Required]
        [StringLength(20)]
        public string Symbol { get; set; } // Currency Symbol (e.g., $)
    }
}
