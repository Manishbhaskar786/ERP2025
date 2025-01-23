using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERPack.Enquiries;
using System;

namespace ERPack.Enquries.Dto;


[AutoMap(typeof(EnquiryMaterial))]
public class EnquiryMaterialDto : EntityDto<int>
{
    public long EnquiryId { get; set; }
    public int MaterialId { get; set; }
    public string MaterialName { get; set; }
    public string ItemCode { get; set; }
    public int SortOrder { get; set; }
}