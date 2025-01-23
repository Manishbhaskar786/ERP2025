using Abp.AutoMapper;
using ERPack.Designs.Dto;
using ERPack.Materials.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ERPack.Web.Models.Production
{
    public class AddEditProductionModel
    {
        public int WorkOrderId { get; set; }
        public int MaterialId { get; set; }
        public string TaskId { get; set; }
        public int SubTaskId { get; set; }
        public string SubTaskSId { get; set; }
        public string CompletionBy { get; set; }
    }
}

