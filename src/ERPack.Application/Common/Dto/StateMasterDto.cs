using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Common.Dto
{
    [AutoMap(typeof(StateMaster))]
    public class StateMasterDto : EntityDto<long>
    {
        public int StateId { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }

        [Required]
        [StringLength(50)]
        public string StateName { get; set; }

        [Required]
        [StringLength(10)]
        public string StateCode { get; set; }

        public virtual CountryMaster Country { get; set; }

        public int? TenantId { get; set; }
    }
}
