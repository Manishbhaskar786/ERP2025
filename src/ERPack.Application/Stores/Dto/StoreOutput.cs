using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Stores.Dto
{
    [AutoMapFrom(typeof(Store))]
    public class StoreOutput
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
    }
}
