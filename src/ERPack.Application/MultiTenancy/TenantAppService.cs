using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using ERPack.Authorization;
using ERPack.Authorization.Roles;
using ERPack.Authorization.Users;
using ERPack.Common.Dto;
using ERPack.Currency;
using ERPack.Customers;
using ERPack.Editions;
using ERPack.Enquiries;
using ERPack.Estimates;
using ERPack.Helpers;
using ERPack.MultiTenancy.Dto;
using ERPack.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.MultiTenancy
{
    public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>, ITenantAppService
    {
        //private readonly IRepository<Tenant, int> _repository;
        private readonly TenantManager _tenantManager;
        private readonly CurrencyManager _currencyManager;
        private readonly HostTenantManager _hostTenantManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly IUserAppService _userAppService;
        private readonly IEmailHelper _emailHelper;
        private readonly IRepository<User, long> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantAppService(
            IUserAppService userAppService,
        IRepository<User, long> userRepository,
            IRepository<Tenant, int> repository,
            TenantManager tenantManager,
            HostTenantManager hostTenantManager,
            EditionManager editionManager,
            CurrencyManager currencyManager,
            UserManager userManager,
            RoleManager roleManager,
            IAbpZeroDbMigrator abpZeroDbMigrator,
            IEmailHelper emailHelper,
            IHttpContextAccessor httpContextAccessor)
            : base(repository)
        {
            //_repository = repository;
            _tenantManager = tenantManager;
            _hostTenantManager = hostTenantManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _currencyManager = currencyManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _userRepository = userRepository;
            _userAppService = userAppService;
            _emailHelper = emailHelper;
            _httpContextAccessor = httpContextAccessor;
            _currencyManager = currencyManager;
        }

        [AbpAuthorize(PermissionNames.Pages_Tenants)]
        public override async Task<TenantDto> CreateAsync(CreateTenantDto input)
        {
            // get web domianUrl 
            var request = _httpContextAccessor.HttpContext.Request;
            string domainUrl = $"{request.Scheme}://{request.Host.Value}";

            CheckCreatePermission();

            // Create tenant
            var tenant = ObjectMapper.Map<Tenant>(input);
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);
            if (tenant.IsActive)
            {
                tenant.Status = (int)TenantStatus.Active;
            }
            else
            {
                tenant.Status = (int)TenantStatus.InActive;
            }
            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }
            bool isEmailisExcist = await _userAppService.checkEmailIsExist(input.AdminEmailAddress);

            if (isEmailisExcist)
            {
                throw new UserFriendlyException(("Email Already Exsist"));
            }

            await _tenantManager.CreateAsync(tenant);
            await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

            // Create tenant database
            _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

            // We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                // Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

                // Grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                // Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
                await _userManager.InitializeOptionsAsync(tenant.Id);

                // set default department id = 1
                adminUser.DepartmentId = 1;
                CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));

                await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

                // Assign admin user to role!
                CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();

                // send Email 
                string resetLink = domainUrl + "/Account/SetPassword?UserId=" + adminUser.Id;
                var template = _emailHelper.GetTenantResetPasswordTemplate(input.TenancyName, resetLink);
                _emailHelper.SendEmail(input.AdminEmailAddress, "Reset Password", template);
            }
            return MapToEntityDto(tenant);
        }

        [AbpAuthorize(PermissionNames.Pages_Tenants)]
        public override async Task<PagedResultDto<TenantDto>> GetAllAsync(PagedTenantResultRequestDto input)
        {
            var tenants = new List<Tenant>();
            input.MaxResultCount = 99999;
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            tenants = query.ToList();

            var tenantData = new List<TenantDto>();
            foreach (var tenant in tenants)
            {
                CurrencyMaster currencyMaster = new();
                var tenantDto = ObjectMapper.Map<TenantDto>(tenant);
                var currency = tenantDto.Currency != null ? await _currencyManager.GetCurrencyByIdAsync(Convert.ToInt32(tenantDto.Currency)) : null ;
                tenantDto.Currency = currency?.Name??null;
                tenantData.Add(tenantDto);
            }
            var result = new PagedResultDto<TenantDto>(query.Count(), tenantData);

            return result;
        }
        protected override IQueryable<Tenant> CreateFilteredQuery(PagedTenantResultRequestDto input)
        {
            return Repository.GetAll().IgnoreQueryFilters()
                              .Where(x => x.IsDeleted == false || x.DeletionTime >= DateTime.Now.AddDays(-90))
                              .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.TenancyName.Contains(input.Keyword) || x.Name.Contains(input.Keyword));
        }

        protected override IQueryable<Tenant> ApplySorting(IQueryable<Tenant> query, PagedTenantResultRequestDto input)
        {
            return query.OrderBy(r => r.Id);
        }

        protected override void MapToEntity(TenantDto updateInput, Tenant entity)
        {
            // Manually mapped since TenantDto contains non-editable properties too.
            entity.Name = updateInput.Name;
            entity.TenancyName = updateInput.TenancyName;
            entity.IsActive = updateInput.IsActive;
            entity.Address1 = updateInput.Address1;
            entity.Address2 = updateInput.Address2;
            entity.City = updateInput.City;
            entity.State = updateInput.State;
            entity.Country = updateInput.Country;
            entity.PinCode = updateInput.PinCode;
            entity.Logo = updateInput.Logo;
            entity.CompanyShortName = updateInput.CompanyShortName;
            entity.DocumentsLogo = updateInput.DocumentsLogo;
        }
        // TODO - admin have not permission for edit tenant
        //[AbpAuthorize(PermissionNames.Pages_EditCompany)]
        [AbpAuthorize(PermissionNames.Pages_Tenants)]
        public override async Task<TenantDto> UpdateAsync(TenantDto input)
        {
            try
            {
                CheckUpdatePermission();
                var entity = await _hostTenantManager.GetAsync();

                var tenant = await _tenantManager.GetByIdAsync(input.Id);
                MapToEntity(input, tenant);
                MapToEntity(input, entity);
                if (tenant.IsActive)
                {
                    tenant.Status = (int)TenantStatus.Active;
                }
                else
                {
                    tenant.Status = (int)TenantStatus.InActive;
                }

                await _hostTenantManager.UpdateAsync(entity);
                await _tenantManager.UpdateAsync(tenant);

                return await GetAsync(input);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void MapToEntity(TenantDto updateInput, HostTenantInfo entity)
        {
            // Manually mapped since TenantDto contains non-editable properties too.
            entity.Address1 = updateInput.Address1;
            entity.Address2 = updateInput.Address2;
            entity.City = updateInput.City;
            entity.State = updateInput.State;
            entity.Country = updateInput.Country;
            entity.PinCode = updateInput.PinCode;
            entity.Logo = updateInput.Logo;
            entity.BankName = updateInput.BankName;
            entity.AccountNumber = updateInput.AccountNumber;
            entity.Branch = updateInput.Branch;
            entity.IFSCCode = updateInput.IFSCCode;
            entity.GSTNumber = updateInput.GSTNumber;
            entity.PANNumber = updateInput.PANNumber;
        }
        public async Task<TenantDto> GetAsync(EntityDto<int> input)
        {
            CheckGetPermission();

            var entity = await GetEntityByIdAsync(input.Id);
            return MapToEntityDto(entity);
        }

        [AbpAuthorize(PermissionNames.Pages_Tenants)]
        public async Task<HostTenantInfo> GetHostTenantInfoAsync()
        {
            var entity = await _hostTenantManager.GetAllAsync();
            return entity.FirstOrDefault();
        }

        [AbpAuthorize(PermissionNames.Pages_Tenants)]
        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var tenant = await _tenantManager.GetByIdAsync(input.Id);
            tenant.Status = (int)TenantStatus.UnderDelation;
            tenant.IsActive = false;
            tenant.IsDeleted = true;
            await _tenantManager.UpdateAsync(tenant);
        }

        [AbpAuthorize(PermissionNames.Pages_Tenants)]
        public async Task restorTenant(EntityDto<int> input)
        {
            CheckDeletePermission();

            var tenant = Repository.GetAll().IgnoreQueryFilters().FirstOrDefault(x => x.Id == input.Id);
            tenant.Status = (int)TenantStatus.Active;
            tenant.IsDeleted = false;
            tenant.IsActive = true;
            await _tenantManager.UpdateAsync(tenant);
        }

        private void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        //public async Task<List<Tenant>> GetAllAsync()
        //{
        //    var tenant = await _repository.GetAllListAsync();

        //    if (tenant == null)
        //    {
        //        //throw new UserFriendlyException("No tenant found!");
        //    }
        //    return tenant;
        //}
        //public async Task DeleteTenantAsync(int tenantId)
        //{
        //    var tenant = await _repository.GetAsync(tenantId);

        //    if (tenant == null)
        //    {
        //        throw new UserFriendlyException("Tenant not found.");
        //    }
        //    if (tenant.DeletionTime.HasValue && tenant.DeletionTime.Value < DateTime.Now.Subtract(TimeSpan.FromDays(90)))
        //    {
        //        await _repository.DeleteAsync(tenant);
        //    }
        //    else
        //    {
        //        throw new UserFriendlyException("Tenant deletion date is not set or not within the 90-day threshold.");
        //    }
        //}


        [AbpAuthorize(PermissionNames.Pages_EditCompany)]
        // [AbpAuthorize(PermissionNames.Pages_Tenants)]
        public  async Task<TenantDto> UpdateTenantAsync(TenantDto input)
        {
            try
            {
                // var tenant = await _tenantManager.GetByIdAsync(input.Id);
                var tenant = await GetEntityByIdAsync(input.Id);// (new EntityDto(AbpSession.TenantId.Value));
                tenant.CompanyShortName = input.CompanyShortName;

                string renamedLogo = !string.IsNullOrEmpty(input.Logo) ? input.Logo.Replace(" ", "_") : null;
                tenant.Logo = renamedLogo;

                string renamedDocumentsLogo = !string.IsNullOrEmpty(input.DocumentsLogo) ? input.DocumentsLogo.Replace(" ", "_") : null;
                tenant.DocumentsLogo = renamedDocumentsLogo;

                tenant.Currency = input.Currency;
                //MapToEntity(input, tenant);
                await _tenantManager.UpdateAsync(tenant);
                await CurrentUnitOfWork.SaveChangesAsync();
                return await GetAsync(input);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

