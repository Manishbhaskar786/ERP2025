using Abp.AutoMapper;

namespace ERPack.Categories.Dto
{
    [AutoMapTo(typeof(Category))]
    public class CategoryDto
    {
        public string CategoryName { get; set; }
        public int? TenantId { get; set; }
        public virtual long? CreatorUserId { get; set; }
    }
}
