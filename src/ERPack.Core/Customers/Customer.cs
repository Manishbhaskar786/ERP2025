using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ERPack.Categories;
using ERPack.Common;
using ERPack.Currency;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Customers
{
    public class Customer : FullAuditedEntity<long>, IMayHaveTenant
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string Name { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [StringLength(170)]
        public string City { get; set; }
        public int StateId { get; set; }
        [StringLength(10)]
        public string PinCode { get; set; }
        public int CountryId { get; set; }
        [StringLength(15)]
        public string Mobile { get; set; }
        [StringLength(100)]
        public string ContactPerson { get; set; }
        [StringLength(100)]
        public string Designation { get; set; }
        [StringLength(20)]
        public string PAN { get; set; }
        [StringLength(20)]
        public string GSTNo { get; set; }
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(15)]
        public string ContactNo { get; set; }
        [StringLength(100)]
        public string Industry { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string Image { get; set; }
        public int? TenantId { get; set; }
        public string CompanyLogo { get; set; }
        public int BusinessCurrencyId { get; set; }
        public virtual CurrencyMaster BusinessCurrency { get; set; }
        public virtual StateMaster State { get; set; }
        public virtual CountryMaster Country { get; set; }
    }
}
