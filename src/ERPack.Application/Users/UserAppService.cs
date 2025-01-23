using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using ERPack.Authorization;
using ERPack.Authorization.Roles;
using ERPack.Authorization.Users;
using ERPack.Identity;
using ERPack.MultiTenancy;
using ERPack.Roles.Dto;
using ERPack.Users.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Users
{

    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IHostEnvironment _env;
        private readonly TenantManager _tenantManager;
        private readonly SignInManager _signInManager;

        public UserAppService(
            SignInManager signInManager,
            TenantManager tenantManager,
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IHostEnvironment env)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _userRepository = repository;
            _env = env;
            _tenantManager = tenantManager;
            _signInManager = signInManager;
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<PagedResultDto<UserDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var users = new List<User>();
            input.MaxResultCount = 99999;
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            users = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            // Fetch the roles for all users
            var usersWithRoles = new List<UserDto>();
            foreach (var user in users)
            {
                var userDto = ObjectMapper.Map<UserDto>(user);
                var userRoles = await _userManager.GetRolesAsync(user);
                userDto.RoleNames = userRoles.ToArray();
                usersWithRoles.Add(userDto);
            }

            var result = new PagedResultDto<UserDto>(query.Count(), usersWithRoles);
            return result;
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<List<UserDto>> GetUsersByDepartmentAsync(int departmentId)
        {
            var users = await _userRepository.GetAllListAsync(x => x.DepartmentId == departmentId);

            var result = ObjectMapper.Map<List<UserDto>>(users);

            return result;
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<UserDto> GetByIdAsync(long userId)
        {
            var user = await _userRepository.GetAll().Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == userId);

            var result = ObjectMapper.Map<UserDto>(user);

            return result;
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        [HttpPost]
        public override async Task<UserDto> CreateAsync([FromForm] CreateUserDto input)
        {
            try
            {
                CheckCreatePermission();

                if (input.ImageFile != null)
                {
                    var file = await SaveFile(input.ImageFile, "UserImage");
                    string fileName = Path.GetFileName(file);
                    input.Image = fileName;
                }
                if (input.AdhaarDocFile != null)
                {
                    var file = await SaveFile(input.AdhaarDocFile, "AdhaarDoc");
                    string fileName = Path.GetFileName(file);
                    input.AdhaarDoc = fileName;
                }
                if (input.PANDocFile != null)
                {
                    var file = await SaveFile(input.PANDocFile, "PANDoc");
                    string fileName = Path.GetFileName(file);
                    input.PANDoc = fileName;
                }

                if (input.AcademicDocsFile != null)
                {
                    var academicDocs = string.Empty;
                    foreach (var f in input.AcademicDocsFile)
                    {
                        var file = await SaveFile(f, "AcademicDoc");
                        string fileName = Path.GetFileName(file);
                        academicDocs = (string.IsNullOrEmpty(academicDocs) ? "" : academicDocs + ",") + fileName;
                    }
                    input.AcademicDocs = academicDocs;
                }

                var user = ObjectMapper.Map<User>(input);

                user.TenantId = AbpSession.TenantId;
                user.IsEmailConfirmed = true;
                user.IsLockoutEnabled = false;

                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

                CheckErrors(await _userManager.CreateAsync(user, input.Password));
                string[] rolesArray = new[] { input.Role };
                if (input.Role != null)
                {
                    CheckErrors(await _userManager.SetRolesAsync(user, rolesArray));
                }

                CurrentUnitOfWork.SaveChanges();

                return MapToEntityDto(user);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        [HttpPost]
        public override async Task<UserDto> UpdateAsync([FromForm] UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            if (input.ImageFile != null)
            {
                var file = await SaveFile(input.ImageFile, "UserImage");
                string fileName = Path.GetFileName(file);
                input.Image = fileName;
            }
            if (input.AdhaarDocFile != null)
            {
                var file = await SaveFile(input.AdhaarDocFile, "AdhaarDoc");
                string fileName = Path.GetFileName(file);
                input.AdhaarDoc = fileName;
            }
            if (input.PANDocFile != null)
            {
                var file = await SaveFile(input.PANDocFile, "PANDoc");
                string fileName = Path.GetFileName(file);
                input.PANDoc = fileName;
            }

            if (input.AcademicDocsFile != null)
            {
                var academicDocs = string.Empty;
                foreach (var f in input.AcademicDocsFile)
                {
                    var file = await SaveFile(f, "AcademicDoc");
                    string fileName = Path.GetFileName(file);
                    academicDocs = (string.IsNullOrEmpty(academicDocs) ? "" : academicDocs + ",") + fileName;
                }
                input.AcademicDocs = academicDocs;
            }

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));
            string[] rolesArray = new[] { input.Role };
            if (input.Role != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, rolesArray));
            }

            return await GetAsync(input);
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var userDto = base.MapToEntityDto(user);
            if (user.Roles != null)
            {
                var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

                var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

                userDto.RoleNames = roles.ToArray();
            }
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles).Include(x => x.Department)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.Id);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }

            var currentUser = await _userManager.GetUserByIdAsync(input.UserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }

            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var uniqueFileName = GetUniqueFileName(file.FileName);
            var dir = Path.Combine(_env.ContentRootPath, "wwwroot\\Images");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filePath = Path.Combine(dir, uniqueFileName);
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            return Path.Combine(@"\Images\", uniqueFileName);
        }

        private async Task<string> SaveFile(IFormFile file, string folderName)
        {
            if (file != null)
            {
                var uniqueFileName = GetUniqueFileName(file.FileName);
                var dir = Path.Combine(_env.ContentRootPath, "wwwroot\\" + folderName);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var filePath = Path.Combine(dir, uniqueFileName);
                await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
                return Path.Combine(@"\" + folderName + "\'", uniqueFileName);
            }

            else
            { return null; }
        }

        public async Task<bool> checkEmailIsExist(string email)
        {
            var query = _userRepository.GetAll().IgnoreQueryFilters();
            var checkEmailIsExist = query.FirstOrDefault(x => x.EmailAddress.ToLower().Trim() == email.ToLower().Trim());

            bool flag = false;
            if (checkEmailIsExist != null)
            {
                flag = true;
            }
            return flag;
        }
        public async Task<bool> SetPassword(SetPasswordDto input)
        {
            var currentUser =  await _userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == input.UserId);

            if (currentUser.IsDeleted) //|| !currentUser.IsActive
            {
                return false;
            }

            if (currentUser != null)
            {
                currentUser.Password = _passwordHasher.HashPassword(currentUser, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            if (currentUser != null && currentUser.TenantId != null)
            {
                var tenant = await _tenantManager.GetByIdAsync(currentUser.TenantId ?? 0);

                var loginResult = await _logInManager.LoginAsync(currentUser.EmailAddress, input.NewPassword, tenant.TenancyName);

                if (currentUser.IsActive && tenant != null && tenant.IsActive)
                {
                    await _signInManager.SignInAsync(loginResult.Identity, true);
                }
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }

            return true;
        }

    }
}

