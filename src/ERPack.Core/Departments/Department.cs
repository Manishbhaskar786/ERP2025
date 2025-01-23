using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ERPack.Departments
{
    public class Department : FullAuditedEntity<int> , IMayHaveTenant
    {
        public int Id { get; set; }
        public string DeptName { get; set; }
        public virtual int? TenantId { get; set; }
        public string Description { get; set; }
    }
}
