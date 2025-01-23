using Abp.AutoMapper;
using ERPack.Materials.ItemTypes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Inventory.Dto
{
    [AutoMap(typeof(InventoryIssuedItem))]
    public class InventoryIssuedItemDto
    {
        public int? TenantId { get; set; }
        public int? ItemTypeId { get; set; }
        public int? MaterialId { get; set; }
        public int? FromStoreId { get; set; }
        public int? ToStoreId { get; set; }
        [Precision(18, 2)]
        public decimal QtyTransferred { get; set; }
        [Precision(18, 2)]
        public decimal SuperStoreQty { get; set; }
        [Precision(18, 2)]
        public decimal StoreQty { get; set; }
        public int? IssuedDepartmentId { get; set; }
        public long PersonIssuedId { get; set; }
        public long InventoryIssueId { get; set; }
        public string ItemType { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string FromStoreName { get; set; }
        public string ToStoreName { get; set; }
        public string DepartmentName { get; set; }
        public string IssuedBy { get; set; }
    }
}
