using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Categories;
using ERPack.Common;
using ERPack.Currency;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Customers.Dto
{
    [AutoMap(typeof(Customer))]
    public class CustomerDto : EntityDto<long>
    {
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
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string Industry { get; set; }
        public IFormFile ImageFile { get; set; }
        public string CompanyLogo { get; set; }
        public int BusinessCurrencyId { get; set; }
        public virtual CurrencyMaster BusinessCurrency { get; set; }
        public virtual StateMaster State { get; set; }
        public virtual CountryMaster Country { get; set; }
        public int? TenantId { get; set; }
    }
}
