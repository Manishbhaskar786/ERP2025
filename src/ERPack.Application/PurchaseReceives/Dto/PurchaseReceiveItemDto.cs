using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.PurchaseRecieves;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.PurchaseReceives.Dto
{
    [AutoMap(typeof(PurchaseReceiveItem))]
    public class PurchaseReceiveItemDto : EntityDto<int>
    {
        public virtual int? TenantId { get; set; }
        public int? PurchaseOrderItemId { get; set; }
        public int? PurchaseReceiveId { get; set; }
        public int? StoreId { get; set; }
        public int? MaterialId { get; set; }
        public int? QuantityOrdered { get; set; }
        public int? QuantityReceived { get; set; }
        public string StoreName { get; set; }
        public string MaterialName { get; set; }
        public string Unit { get; set; }
    }
}
