using Abp.AutoMapper;
using ERPack.Materials.ItemTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Inventory.Dto
{
    [AutoMap(typeof(InventoryIssued))]
    public class InventoryIssuedDto
    {
        public long Id { get; set; }
        public int? TenantId { get; set; }
        public string IssueCode { get; set; }
        public string MaterialIssueSlipPath { get; set; }
        public int? InventoryRequestId { get; set; }
        public bool IsManual { get; set; }
        public DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }
        public string IssuedBy { get; set; }
    }
}
