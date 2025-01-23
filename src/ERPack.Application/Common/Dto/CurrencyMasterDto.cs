using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Currency;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Common.Dto
{
    [AutoMap(typeof(CurrencyMaster))]
    public class CurrencyMasterDto : EntityDto<long>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
    }
}
