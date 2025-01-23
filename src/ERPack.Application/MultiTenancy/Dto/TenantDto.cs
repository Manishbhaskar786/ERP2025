using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using ERPack.Currency.Dto;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Cmp;

namespace ERPack.MultiTenancy.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantDto : EntityDto
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(AbpTenantBase.TenancyNameRegex , ErrorMessage = "Tenancy Name must start with a letter and can contain letters, numbers, hyphens, and underscores.")]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }        
        
        public bool IsActive {get; set;}
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
        [StringLength(50)]
        public string IFSCCode { get; set; }
        [StringLength(100)]
        public string GSTNumber { get; set; }
        [StringLength(100)]
        public string PANNumber { get; set; }
        [StringLength(100)]
        public string CompanyShortName { get; set; }
        public string DocumentsLogo { get; set; }

        public IFormFile DocumentsLogoFile { get; set; }

        public string Currency { get; set; }
        public List<CurrencyOutput> Currencies { get; set; }
        
        public int Status { get; set; }
        public DateTime DeletionTime { get; set; }
    }
}
