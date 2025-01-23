using Abp.AutoMapper;
using Abp.Runtime.Validation;
using ERPack.Authorization.Users;


namespace ERPack.Stores.Dto
{
    [AutoMap(typeof(Store))]
    public class StoreDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public int? TenantId { get; set; }
        public virtual long? CreatorUserId { get; set; }
    }
}
