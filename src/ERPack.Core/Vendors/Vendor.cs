using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Vendors
{
    public class Vendor : FullAuditedEntity<int> , IMayHaveTenant
    {
        [StringLength(150)]
        public string VendorName { get; set; }
        [StringLength(15)]
        public string VendorCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [StringLength(170)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(10)]
        public string PinCode { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        [StringLength(150)]
        public string ContactPerson { get; set; }
        [StringLength(150)]
        public string Designation { get; set; }
        [StringLength(50)]
        public string GST { get; set; }
        [StringLength(150)]
        public string BankName { get; set; }
        [StringLength(50)]
        public string AccountNumber { get; set; }
        [StringLength(200)]
        public string Branch { get; set; }
        [StringLength(50)]
        public string IFSC { get; set; }
        [StringLength(20)]
        public string PanCard { get; set; }
        [StringLength(15)]
        public string PhoneNo { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentUrl { get; set; }
        public virtual int? TenantId { get; set; }
    }
}
