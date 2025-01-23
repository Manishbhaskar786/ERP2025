using Abp.AutoMapper;
using ERPack.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Preferences.Dto
{
    [AutoMap(typeof(Preference))]
    public class PreferenceDto
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
