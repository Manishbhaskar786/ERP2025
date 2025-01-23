using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace ERPack.Workorders.Dto
{
    [AutoMap(typeof(WorkorderTask))]
    public class WorkorderTaskDto : EntityDto<int>
    {
        public int Id { get; set; }
        public long? WorkorderId { get; set; }
        public string WorkorderCode { get; set; }
        public string WorkOrderTaskId { get; set; }
        public long? WorkorderSubTaskId { get; set; }
        public int WorkorderSubTaskCode { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public DateTime? TaskIssueDate { get; set; }
        public DateTime? TaskIssueCompleteDate { get; set; }
        public DateTime? TaskIssueActualCompleteDate { get; set; }
        public int UnitId { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string MaterialName { get; set; }
        public string ItemCode { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SellingUnitName { get; set; }
        public string Status { get; set; }
        public int? TenantId { get; set; }
    }
}
