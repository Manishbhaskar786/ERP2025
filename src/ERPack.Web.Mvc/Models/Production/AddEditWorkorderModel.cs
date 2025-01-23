using Abp.AutoMapper;
using ERPack.Designs.Dto;
using ERPack.Estimates.Dto;
using ERPack.Materials.Dto;
using ERPack.Units.Dto;
using ERPack.Workorders.Dto;
using System.Collections.Generic;

namespace ERPack.Web.Models.Production
{
    [AutoMap(typeof(WorkorderDto))]
    public class AddEditWorkorderModel
    {
        public int Id { get; set; }
        public long EstimateId { get; set; }
        public string EstimateCode { get; set; }
        public string Description { get; set; }
        public string WorkorderId { get; set; }
        public bool? IsHighPriority { get; set; }
        public IReadOnlyList<EstimateDto> Estimates { get; set; }
        public IReadOnlyList<MaterialDto> Materials { get; set; }
        public IReadOnlyList<UnitOutput> Units { get; set; }
        public IReadOnlyList<WorkorderTaskDto> WorkorderTasks { get; set; }
        public IReadOnlyList<WorkorderSubTaskDto> WorkorderSubTasks { get; set; }
    }
}

