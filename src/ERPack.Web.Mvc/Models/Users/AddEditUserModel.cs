using Abp.AutoMapper;
using ERPack.Common.Dto;
using ERPack.Departments.Dto;
using ERPack.Roles.Dto;
using ERPack.Users.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERPack.Web.Models.Users
{
    [AutoMapFrom(typeof(UserDto))]
    public class AddEditUserModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public DateTime DOB { get; set; }

        public string Gender { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
        public string Country { get; set; }
        public string Mobile { get; set; }
        public DateTime DOJ { get; set; }
        public string Designation { get; set; }
        public int DepartmentId { get; set; }
        public string AdhaarNumber { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string[] RoleNames { get; set; }
        public IReadOnlyList<RoleDto> Roles { get; set; }
        public IReadOnlyList<DepartmentOutput> Departments { get; set; }
        public IReadOnlyList<CountryMasterDto> Countries { get; set; }
        public IReadOnlyList<StateMasterDto> States { get; set; }
        public bool UserIsInRole(RoleDto role)
        {
            return RoleNames != null && RoleNames.Any(r => r == role.NormalizedName);
        }

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
