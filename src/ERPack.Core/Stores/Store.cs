using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ERPack.Stores
{
    public class Store : FullAuditedEntity<int> , IMayHaveTenant
    {
        [StringLength(100)]
        public string StoreName { get; set; }
        [StringLength(100)]
        public string StoreLocation { get; set; }
        public virtual int? TenantId { get; set; }
    }
}
