using Abp.AutoMapper;


namespace ERPack.Departments.Dto
{
    [AutoMapTo(typeof(Department))]
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string DeptName { get; set; }
        public int? TenantId { get; set; }
        public virtual long? CreatorUserId { get; set; }
        public string Description { get; set; }
    }
}
