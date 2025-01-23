using Abp.AutoMapper;
using ERPack.Materials.Units;

namespace ERPack.Units.Dto
{
    [AutoMapFrom(typeof(Unit))]
    public class UnitOutput
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
    }
}
