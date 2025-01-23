using Abp.AutoMapper;
using ERPack.Common.Dto;
using ERPack.Customers.Dto;
using ERPack.MultiTenancy.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Web.Models.HostTaxationInfo
{
    [AutoMap(typeof(HostTaxationDto))]
    public class AddEditHostTaxationDetailsModel
    {
        public int Id { get; set; }
        [Required]
        public string AddressName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [StringLength(170)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(10)]
        public string PinCode { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        [StringLength(20)]
        public string GSTNumber { get; set; }
        [StringLength(20)]
        public string PANNumber { get; set; }
        public bool IsDefault { get; set; }
        public virtual int? TenantId { get; set; }

        public IReadOnlyList<CountryMasterDto> Countries { get; set; }
        public IReadOnlyList<StateMasterDto> States { get; set; }
    }
}
