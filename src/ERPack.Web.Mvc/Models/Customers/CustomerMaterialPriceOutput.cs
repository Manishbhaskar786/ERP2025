using Abp.AutoMapper;
using ERPack.Customers.CustomerMaterialPrices;
using ERPack.Customers.Dto;

namespace ERPack.Web.Models.Customers
{
    [AutoMap(typeof(CustomerMaterialPriceDto))]
    public class CustomerMaterialPriceOutput
    {
        public long? CustomerId { get; set; }
        public int? MaterialId { get; set; }
        public int? UnitId { get; set; }
        public decimal? Price { get; set; }
    }
}
