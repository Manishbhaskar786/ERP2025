using Abp.AutoMapper;
using ERPack.Departments.Dto;
using ERPack.Inventory.Dto;
using ERPack.Materials.Dto;
using ERPack.PurchaseOrders.Dto;
using ERPack.PurchaseReceives.Dto;
using ERPack.Stores.Dto;
using ERPack.Units.Dto;
using ERPack.Users.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ERPack.Web.Models.PurchaseReceive
{
    [AutoMapTo(typeof(PurchaseReceiveDto))]
    public class AddEditPurchaseReceiveModel
    {
        public int? Id { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? TenantId { get; set; }
        public int? VendorId { get; set; }
        public DateTime? PurchaseReceiveDate { get; set; }
        public IReadOnlyList<StoreDto> Stores { get; set; }
        public IReadOnlyList<PurchaseOrderDto> PurchaseOrders { get; set; }
        public IReadOnlyList<PurchaseReceiveItemDto> PurchaseReceiveItems { get; set; }
    }
}
