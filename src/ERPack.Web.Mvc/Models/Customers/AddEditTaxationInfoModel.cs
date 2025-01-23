using Abp.AutoMapper;
using ERPack.Common.Dto;
using ERPack.Customers.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Web.Models.Customers
{
    [AutoMap(typeof(CustomerTaxationInfoDto))]
    public class AddEditTaxationInfoModel
    {
        public int Id { get; set; }
        public string AddressName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [StringLength(170)]
        public string City { get; set; }
        public int StateId { get; set; }
        [StringLength(10)]
        public string PinCode { get; set; }
        public int CountryId { get; set; }
        [StringLength(20)]
        public string GSTNumber { get; set; }
        [StringLength(20)]
        public string PANNumber { get; set; }
        public bool IsDefault { get; set; }
        public int? TenantId { get; set; }
        public long CustomerId { get; set; }
        public IReadOnlyList<StateMasterDto> States { get; set; }
        public IReadOnlyList<CountryMasterDto> Countries { get; set; }
    }
}
