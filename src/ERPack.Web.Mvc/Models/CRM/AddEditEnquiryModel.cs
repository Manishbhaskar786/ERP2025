using Abp.AutoMapper;
using ERPack.Enquiries.Dto;
using ERPack.Enquries.Dto;
using ERPack.Materials.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Web.Models.CRM
{
    [AutoMap(typeof(EnquiryDto))]
    public class AddEditEnquiryModel
    {
        public long Id { get; set; }
        public string EnquiryId { get; set; }
        public long? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public long? DesignUserId { get; set; }
        public string DesignName { get; set; }
        public string DesignNumber { get; set; }
        public decimal? BoxLength { get; set; }
        public decimal? BoxWidth { get; set; }
        public decimal? BoxHeight { get; set; }
        public decimal? SheetSizeLength { get; set; }
        public decimal? SheetSizeWidth { get; set; }
        public int? BoardTypeId { get; set; }
        public string BoardTypeName { get; set; }
        public int? ToolTypeId { get; set; }
        public int? ToolConfigurationId { get; set; }
        public int? ToolFamilyId { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDatetime { get; set; }
        public bool IsHighPriority { get; set; }
        public IFormFile DesignImageDoc { get; set; }
        public string DesignImage { get; set; }
        public bool IsBraile { get; set; }
        public decimal? BraileLength { get; set; }
        public decimal? BraileWidth { get; set; }
        public string BraileComments { get; set; }
        public bool IsEmboss { get; set; }
        public decimal? EmbossLength { get; set; }
        public decimal? EmbossWidth { get; set; }
        public string EmbossComments { get; set; }
        public int? NumberOfUps { get; set; }
        public IReadOnlyList<EnquiryMaterialDto> EnquiryMaterials { get; set; }
        public IReadOnlyList<MaterialDto> Materials { get; set; }
    }
}

