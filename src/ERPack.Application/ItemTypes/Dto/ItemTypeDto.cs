using Abp.AutoMapper;
using ERPack.Materials.ItemTypes;

namespace ERPack.Units.Dto
{
    [AutoMapTo(typeof(ItemType))]
    public class ItemTypeDto
    {
        public string ItemTypeName { get; set; }
    }
}
