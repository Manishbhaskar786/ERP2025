using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using ERPack.Authorization.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto 
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string Gender { get; set; }

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
        [StringLength(15)]
        public string Mobile { get; set; }
        public DateTime DOJ { get; set; }
        [StringLength(100)]
        public string Designation { get; set; }
        public int DepartmentId { get; set; }
        [StringLength(14)]
        public string AdhaarNumber { get; set; }
        public string Image { get; set; }
        public string AdhaarDoc { get; set; }
        public string PANDoc { get; set; }
        public string AcademicDocs { get; set; }

        public List<IFormFile> AcademicDocsFile { get; set; }
        public IFormFile AdhaarDocFile { get; set; }
        public IFormFile ImageFile { get; set; }
        public IFormFile PANDocFile { get; set; }

    }
}
