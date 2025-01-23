using Abp.AutoMapper;
using ERPack.Categories.Dto;
using ERPack.Departments.Dto;
using ERPack.Materials.Dto;
using ERPack.Units.Dto;
using ERPack.Users.Dto;
using System.Collections.Generic;

namespace ERPack.Web.Models.Materials
{
    [AutoMapFrom(typeof(MaterialDto))]
    public class AddEditMaterialModel
    {
        public int Id { get; set; }
        public int ItemTypeId { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public int BuyingUnitId { get; set; }
        public int SellingUnitId { get; set; }
        public int MaterialTypeId { get; set; }
        public int UnitId { get; set; }
        public string ItemCode { get; set; }
        public string HSN { get; set; }
        public decimal? CGST { get; set; }
        public decimal? SGST { get; set; }
        public decimal? IGST { get; set; }
        public string Qty { get; set; }
        public string BuyingPrice { get; set; }
        public string SellingPrice { get; set; }
        public string MaterialName { get; set; }
        public int? DepartmentId { get; set; }
        public int? CategoryId { get; set; }
        public IReadOnlyList<DepartmentOutput> Departments { get; set; }
        public IReadOnlyList<ItemTypeOutput> ItemTypes { get; set; }
        public IReadOnlyList<UnitOutput> Units { get; set; }
        public IReadOnlyList<CategoryOutput> Categories { get; set; }
    }
}

