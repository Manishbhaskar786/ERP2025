using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Customers.CustomerMaterialPrices;
using Microsoft.AspNetCore.Http;

namespace ERPack.Customers.Dto
{
    [AutoMap(typeof(CustomerMaterialPrice))]
    public class CustomerMaterialPriceDto : EntityDto<long>
    {
        public int? TenantId { get; set; }
        public long CustomerId { get; set; }
        public int MaterialId { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
    }
}
