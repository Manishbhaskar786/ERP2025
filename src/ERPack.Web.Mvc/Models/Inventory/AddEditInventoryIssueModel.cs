using Abp.AutoMapper;
using ERPack.Departments.Dto;
using ERPack.Estimates.Dto;
using ERPack.Inventory.Dto;
using ERPack.Materials.Dto;
using ERPack.Stores.Dto;
using ERPack.Units.Dto;
using ERPack.Users.Dto;
using System.Collections.Generic;

namespace ERPack.Web.Models.Inventory
{
    [AutoMap(typeof(InventoryIssuedDto))]
    public class AddEditInventoryIssueModel
    {
        public long Id { get; set; }
        public int? TenantId { get; set; }
        public string IssueCode { get; set; }
        public int? InventoryRequestId { get; set; }
        public string MaterialIssueSlipPath { get; set; }
        public bool IsManual { get; set; }
        public List<InventoryRequestOutput> InventoryRequests { get; set; }
        public IReadOnlyList<DepartmentOutput> Departments { get; set; }
        public IReadOnlyList<ItemTypeOutput> ItemTypes { get; set; }
        public IReadOnlyList<MaterialDto> Materials { get; set; }
        public IReadOnlyList<StoreDto> Stores { get; set; }
        public IReadOnlyList<InventoryIssuedItemDto> InventoryItems { get; set; }
    }
}
