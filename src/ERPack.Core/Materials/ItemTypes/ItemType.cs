using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Materials.ItemTypes
{
    public class ItemType : Entity<int>
    {
        public string ItemTypeName { get; set; }
    }
}
