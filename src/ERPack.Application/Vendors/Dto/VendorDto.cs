using Abp.AutoMapper;
using ERPack.Vendors;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Vendors.Dto
{
    [AutoMap(typeof(Vendor))]
    public class VendorDto
    {
        public int Id { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
        public string Country { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string GST { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        public string IFSC { get; set; }
        public string PanCard { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentUrl { get; set; }
        public int? TenantId { get; set; }
        public virtual long? CreatorUserId { get; set; }
    }
}
