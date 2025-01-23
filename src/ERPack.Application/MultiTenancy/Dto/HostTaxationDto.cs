using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.HostTaxation;
using Microsoft.AspNetCore.Http;

namespace ERPack.MultiTenancy.Dto
{
    [AutoMap(typeof(HostTaxationInfo))]
    public class HostTaxationDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Please Enter Address Name")]
        public string AddressName { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        [StringLength(170)]
        public string City { get; set; }
        [Required]
        [StringLength(50)]
        public string State { get; set; }
        [Required]
        [StringLength(10)]
        public string PinCode { get; set; }
        [Required]
        [StringLength(100)]
        public string Country { get; set; }
        [Required]
        [StringLength(20)]
        public string GSTNumber { get; set; }
        [Required]
        [StringLength(20)]
        public string PANNumber { get; set; }
        public bool IsDefault { get; set; }
        public virtual int? TenantId { get; set; }
    }
}
