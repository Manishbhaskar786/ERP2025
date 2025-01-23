using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Materials.MaterialInventories;
using Microsoft.EntityFrameworkCore;

namespace ERPack.Materials.Dto
{
    [AutoMap(typeof(MaterialInventory))]
    public class MaterialInventoryDto : EntityDto<int>
    {
        public long MaterialId { get; set; }
        public int StoreId { get; set; }
        [Precision(18, 2)]
        public decimal Quantity { get; set; }
    }
}
