using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ERPack.Customers.CustomerMaterialPrices
{
    public class CustomerMaterialPrice : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int CustomerId { get; set; }
        public long MaterialId { get; set; }
        public int UnitId { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
