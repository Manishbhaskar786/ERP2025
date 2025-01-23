using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERPack.Categories
{
    public class Category : FullAuditedEntity<int> , IMayHaveTenant
    {
        public string CategoryName { get; set; }
        public virtual int? TenantId { get; set; }
    }
}
