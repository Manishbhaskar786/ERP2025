using Abp.AutoMapper;
using ERPack.Materials.ItemTypes;
using ERPack.Materials.Units;

namespace ERPack.Units.Dto
{
    [AutoMapFrom(typeof(ItemType))]
    public class ItemTypeOutput
    {
        public int Id { get; set; }
        public string ItemTypeName { get; set; }
    }
}
