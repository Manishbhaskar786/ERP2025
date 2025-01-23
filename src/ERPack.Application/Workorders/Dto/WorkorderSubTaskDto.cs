using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace ERPack.Workorders.Dto
{
    [AutoMap(typeof(WorkorderSubTask))]
    public class WorkorderSubTaskDto : EntityDto<int>
    {
        public long Id { get; set; }
        public string WorkOrderSubTaskId { get; set; }
        public long? WorkorderId { get; set; }
        public int DepartmentId { get; set; }
        public string Status { get; set; }
        public int? TenantId { get; set; }
    }
}
