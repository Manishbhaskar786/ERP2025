using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ERPack.Materials.Dto
{
    [AutoMap(typeof(Material))]
    public class MaterialDto : EntityDto<int>
    {
        public int ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public int BuyingUnitId { get; set; }
        public int SellingUnitId { get; set; }
        public int? CategoryId { get; set; }
        public string ItemCode { get; set; }
        public string HSN { get; set; }
        public decimal? CGST { get; set; }
        public decimal? SGST { get; set; }
        public decimal? IGST { get; set; }
        public string BuyingUnit { get; set; }
        public string SellingUnit { get; set; }
        public string BuyingPrice { get; set; }
        public string SellingPrice { get; set; }
        public string DepartmentName { get; set; }
        public int? DepartmentId { get; set; }
        public string Industry { get; set; }
        public int? TenantId { get; set; }
    }
}
