using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Enquiries.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Designs.Dto
{
    [AutoMap(typeof(DesignMaterial))]
    public class DesignMaterialDto
    {
        public int? Id { get; set; }
        public long DesignId { get; set; }
        public int MaterialId { get; set; }
        public decimal? Quantity { get; set; }
        public string SellingPrice { get; set; }
        public string DisplayName { get; set; }
        public string ItemCode { get; set; }
        public int SellingUnitId { get; set; }
        public decimal? CGST { get; set; }
        public decimal? SGST { get; set; }
        public decimal? IGST { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
    }
}
