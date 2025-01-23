using Abp.AutoMapper;
using ERPack.Designs.Dto;
using ERPack.Enquiries.Dto;
using ERPack.Estimates.Dto;
using ERPack.Materials.Dto;
using ERPack.Stores.Dto;
using ERPack.Units.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ERPack.Web.Models.CRM
{
    [AutoMap(typeof(EstimateDto))]
    public class AddEditEstimateModel
    {
        public long Id { get; set; }
        public string EstimateId { get; set; }
        public string Description { get; set; }
        public long? DesignId { get; set; }
        public long? EnquiryId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public IReadOnlyList<DesignDto> CompletedDesigns { get; set; }
        public IReadOnlyList<MaterialDto> Materials { get; set; }
        public IReadOnlyList<UnitOutput> Units { get; set; }
        public IReadOnlyList<EstimateTaskDto> EstimateTasks { get; set; }
    }
}

