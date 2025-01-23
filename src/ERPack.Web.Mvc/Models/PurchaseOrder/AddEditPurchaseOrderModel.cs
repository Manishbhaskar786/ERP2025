using Abp.AutoMapper;
using ERPack.Departments.Dto;
using ERPack.Inventory.Dto;
using ERPack.Materials.Dto;
using ERPack.Materials.Units;
using ERPack.PurchaseOrders;
using ERPack.PurchaseOrders.Dto;
using ERPack.Stores.Dto;
using ERPack.Units.Dto;
using ERPack.Users.Dto;
using ERPack.Vendors.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ERPack.Web.Models.PurchaseOrder
{
    [AutoMapTo(typeof(PurchaseOrderDto))]
    public class AddEditPurchaseOrderModel
    {
        public int? Id { get; set; }
        public int? TenantId { get; set; }
        public int? VendorId { get; set; }
        public string POCode { get; set; }
        public DateTime PurchaseDate { get; set; }
        public IReadOnlyList<MaterialDto> Materials { get; set; }
        public IReadOnlyList<VendorDto> Vendors { get; set; }
        public IReadOnlyList<ItemTypeOutput> ItemTypes { get; set; }
        public IReadOnlyList<UnitOutput> Units { get; set; }
        public IReadOnlyList<PurchaseOrderItemDto> PurchaseOrderItems { get; set; }
    }
}
