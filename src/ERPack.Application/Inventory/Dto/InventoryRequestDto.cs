using Abp.AutoMapper;

namespace ERPack.Inventory.Dto
{
    [AutoMap(typeof(InventoryRequest))]
    public class InventoryRequestDto
    {
        public string Id { get; set; }
        public string InventoryRequestId { get; set; }
        public int MaterialId { get; set; }
        public long RequestFromUserId { get; set; }
        public decimal ReqQty { get; set; }
        public int? DepartmentId { get; set; }
        public string Remark { get; set; }
        public bool? IsReqClose { get; set; }
        public long? WorkorderTaskId { get; set; }
    }
}
