using Abp.AutoMapper;
using ERPack.Designs.Dto;
using ERPack.Materials.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ERPack.Web.Models.Production
{
    [AutoMap(typeof(DesignDto))]
    public class AddEditDesignModel
    {
        public long? Id { get; set; }
        public long EnquiryId { get; set; }
        public string DesignId { get; set; }
        public string Comments { get; set; }
        public string DesignName { get; set; }
        public string DesignNumber { get; set; }
        public string DesignImage { get; set; }
        public decimal? BoxLength { get; set; }
        public decimal? BoxWidth { get; set; }
        public decimal? BoxHeight { get; set; }
        public decimal? SheetSizeLength { get; set; }
        public decimal? SheetSizeWidth { get; set; }
        public DateTime? CompletionDatetime { get; set; }
        public string Status { get; set; }
        public int BoardTypeId { get; set; }
        public IReadOnlyList<MaterialDto> Materials { get; set; }
        public IReadOnlyList<DesignMaterialDto> DesignMaterials { get; set; }
        public virtual long? CustomerId { get; set; }
        public string EnquiryNumber { get; set; }
        public IFormFile DesignImageDoc { get; set; }
        public string ReportDoc { get; set; }
    }
}