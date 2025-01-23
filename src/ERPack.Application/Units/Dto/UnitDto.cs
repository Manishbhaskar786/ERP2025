using Abp.AutoMapper;
using ERPack.Materials.Units;

namespace ERPack.Units.Dto
{
    [AutoMapTo(typeof(Unit))]
    public class UnitDto
    {
        public string UnitName { get; set; }
        public int? TenantId { get; set; }
        public virtual long? CreatorUserId { get; set; }
    }
}
