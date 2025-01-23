using Abp.MultiTenancy;
using ERPack.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace ERPack.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
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
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string PANNumber { get; set; }
        public string Logo { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        public string IFSCCode { get; set; }
        [StringLength(100)]
        public string GSTNumber { get; set; }

        [StringLength(100)]
        public string CompanyShortName { get; set; }
        public string DocumentsLogo { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; }
        public Tenant()
        {
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
