using Abp.AutoMapper;
using ERPack.Preferences.Dto;

namespace ERPack.Web.Models.Preferences
{
    [AutoMapFrom(typeof(PreferenceDto))]
    public class AddEditPreferenceModel
    {
        public int Id { get; set; }
        public string FrontStyle { get; set; }
        public string FrontSize { get; set; }
        public string IdType { get; set; }
        public string NameIdentifier { get; set; }
        public string FixedName { get; set; }
        public string MonthSelection { get; set; }
        public string YearSelection { get; set; }
        public string DisplayId { get; set; }
        public virtual int? TenantId { get; set; }
    }
}
