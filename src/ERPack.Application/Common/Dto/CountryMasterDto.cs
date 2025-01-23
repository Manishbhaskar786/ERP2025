using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Common.Dto
{
    [AutoMap(typeof(CountryMaster))]
    public class CountryMasterDto : EntityDto<long>
    {
        public int CountryId { get; set; }

        [Required]
        [StringLength(250)]
        public string CountryName { get; set; }

        [Required]
        [StringLength(10)]
        public string CountryCode { get; set; }

        public int? TenantId { get; set; }
    }
}
