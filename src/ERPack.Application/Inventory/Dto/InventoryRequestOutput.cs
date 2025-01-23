using Abp.AutoMapper;

namespace ERPack.Inventory.Dto
{
    [AutoMapFrom(typeof(InventoryRequest))]
    public class InventoryRequestOutput
    {
        public string Id { get; set; }
        public string InventoryRequestId { get; set; }
        public int MaterialId { get; set; }
        public int RequestFromUserId { get; set; }
        public string RequestedBy { get; set; }
    }
}
