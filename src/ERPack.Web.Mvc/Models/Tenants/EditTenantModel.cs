using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using ERPack.Currency.Dto;
using ERPack.Customers.Dto;
using ERPack.MultiTenancy.Dto;
using ERPack.Vendors.Dto;
using Microsoft.AspNetCore.Http;

namespace ERPack.Web.Models.Vendors
{
    [AutoMapTo(typeof(TenantDto))]
    public class EditTenantModel
    {
        public int Id { get; set; }
        public string TenancyName { get; set; }
        public string Name { get; set; }
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
        public string PANNumber { get; set; }

        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        [StringLength(50)]
        public string IFSCCode { get; set; }
        [StringLength(100)]
        public string GSTNumber { get; set; }
        public string CompanyShortName { get; set; }
        public string DocumentsLogo { get; set; }
        public IFormFile DocumentsLogoFile { get; set; }
        public string Currency { get; set; }
        public List<CurrencyOutput> Currencies { get; set; }
    }
}
