using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;
using ERPack.Departments;

namespace ERPack.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public DateTime DOB { get; set; }

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
        public virtual Department Department { get; set; }
        [StringLength(14)]
        public string AdhaarNumber { get; set; }
        public string Image { get; set; }
        public string AdhaarDoc { get; set; }
        public string PANDoc { get; set; }
        public string AcademicDocs { get; set; }


        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
