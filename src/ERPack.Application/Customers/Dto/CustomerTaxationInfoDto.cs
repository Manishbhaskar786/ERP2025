using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Common;
using ERPack.Customers.CustomerTaxationInfos;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Customers.Dto
{
    [AutoMap(typeof(CustomerTaxationInfo))]
    public class CustomerTaxationInfoDto : EntityDto<long>
    {
        public int Id { get; set; }
        [Required]
        public string AddressName { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [StringLength(170)]
        [Required]
        public string City { get; set; }
        [Required]
        public int StateId { get; set; }
        public virtual StateMaster State { get; set; }
        [StringLength(10)]
        [Required]
        public string PinCode { get; set; }
        [Required]
        public int CountryId { get; set; }
        public virtual CountryMaster Country { get; set; }
        [StringLength(20)]
        [Required]
        public string GSTNumber { get; set; }
        [StringLength(20)]
        [Required]
        public string PANNumber { get; set; }
        public bool IsDefault { get; set; }
        public int? TenantId { get; set; }
        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
