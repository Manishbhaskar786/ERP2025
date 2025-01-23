using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ERPack.MultiTenancy.Dto
{
    [AutoMapFrom(typeof(HostTenantInfo))]
    public class HostTenantDto : EntityDto
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
        public string Logo { get; set; }
        public IFormFile LogoFile { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        public string IFSCCode { get; set; }
        [StringLength(100)]
        public string GSTNumber { get; set; }
        [StringLength(100)]
        public string PANNumber { get; set; }
    }
}
