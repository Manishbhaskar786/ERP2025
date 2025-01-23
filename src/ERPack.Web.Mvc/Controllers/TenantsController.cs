using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.MultiTenancy;
using ERPack.MultiTenancy.Dto;
using ERPack.Web.Models.Vendors;
using ERPack.Customers.Dto;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;
using ERPack.Designs;
using ERPack.Customers;
using Abp.Logging;
using ERPack.Web.Models.HostTaxationInfo;
using ERPack.Common;
using ERPack.HostTaxationInfoDetails;
using System.Linq;
using Microsoft.AspNetCore.Server.IISIntegration;
using static ERPack.Authorization.Roles.StaticRoleNames;

namespace ERPack.Web.Controllers
{
    public class TenantsController : ERPackControllerBase
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly IHostEnvironment _env;
        private readonly ICurrencyAppService _currencyAppService;
        private readonly IHostTaxationInfoAppService _hostTaxationInfoAppService;
        private readonly ICountryMasterAppService _countryMasterAppService;
        private readonly IStateMasterAppService _stateMasterAppService;

        public TenantsController(ITenantAppService tenantAppService,
            IHostEnvironment env,
            ICurrencyAppService currencyAppService,
            IHostTaxationInfoAppService hostTaxationInfoAppService,
            ICountryMasterAppService countryMasterAppService,
            IStateMasterAppService stateMasterAppService)
        {
            _tenantAppService = tenantAppService;
            _env = env;
            _currencyAppService = currencyAppService;
            _hostTaxationInfoAppService = hostTaxationInfoAppService;
            _countryMasterAppService = countryMasterAppService;
            _stateMasterAppService = stateMasterAppService;
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
        public ActionResult Index() => View();

        [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
        public async Task<ActionResult> EditModal(int tenantId)
        {
            try
            {
                var tenantDto = await _tenantAppService.GetAsync(new EntityDto(tenantId));
                return PartialView("_EditModal", tenantDto);
            }
            catch (Exception ex)
            {
                return PartialView("_EditModal");
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_EditCompany)]
        public async Task<ActionResult> Edit()
        {
            if (AbpSession.TenantId != null)
            {
                var tenantDto = await _tenantAppService.GetAsync(new EntityDto(AbpSession.TenantId.Value));
                tenantDto.Currencies = await _currencyAppService.GetAllAsync();
                return View(tenantDto);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> AddEditCompanyTaxationDetails(int hostTaxationId = 0, string returnUrl = "")
        {
            AddEditHostTaxationDetailsModel model = new AddEditHostTaxationDetailsModel();
            if (hostTaxationId > 0)
            {
                HostTaxationDto hostTaxationDto = await _hostTaxationInfoAppService.GetAsync(hostTaxationId);
                model = ObjectMapper.Map<AddEditHostTaxationDetailsModel>(hostTaxationDto);
            }
            model.States = await _stateMasterAppService.GetStatesAsync();
            model.Countries = await _countryMasterAppService.GetCountriesAsync();
            return PartialView("_AddEditCompanyTaxationDetails", model);
        }

        [HttpPost]
        public async Task<ActionResult> AddEditCompanyTaxationDetails(AddEditHostTaxationDetailsModel input)
        {
            var errMsg = string.Empty;
            try
            {
                HostTaxationDto customerDto = ObjectMapper.Map<HostTaxationDto>(input);
                var hostInfoDetails = await _hostTaxationInfoAppService.GetTaxationDetailsByTenantId((int)AbpSession.TenantId);
                if (input.Id == 0)
                {
                    if (hostInfoDetails != null)
                    {
                        if (hostInfoDetails.Any(x => x.PANNumber.ToUpper().Equals(input.PANNumber.ToUpper())))
                        {
                            return Json(new
                            {
                                success = false,
                                msg = "Can not insert duplicate PAN number",
                                id = 0
                            });
                        }
                    }
                    customerDto.TenantId = AbpSession.TenantId;
                    (var taxationId, errMsg) = await _hostTaxationInfoAppService.CreateAsync(customerDto);
                    return Json(new
                    {
                        success = true,
                        msg = errMsg,
                        id = taxationId
                    });
                }
                else
                {
                    if (hostInfoDetails != null)
                    {
                        if (hostInfoDetails.Any(x => x.Id != input.Id && x.PANNumber.ToUpper().Equals(input.PANNumber.ToUpper())))
                        {
                            return Json(new
                            {
                                success = false,
                                msg = "Can not insert duplicate PAN number",
                                id = 0
                            });
                        }
                    }
                    (var customer, errMsg) = await _hostTaxationInfoAppService.UpdateAsync(customerDto);
                    return Json(new
                    {
                        success = true,
                        msg = errMsg,
                        id = customer.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error in creating customer", ex);
                return Json(new
                {
                    success = false,
                    msg = errMsg,
                    id = 0
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_EditCompany)]
        public async Task<JsonResult> Update(EditTenantModel input)
        {
            try
            {
                TenantDto tenantDto = ObjectMapper.Map<TenantDto>(input);
                tenantDto.Currency = input.Currency.ToString();

                if (input.LogoFile != null)
                {
                    var logo = await SaveFile(tenantDto.LogoFile);
                    string fileName = Path.GetFileName(logo);
                    tenantDto.Logo = fileName;
                    var documentFile = await SaveFile(tenantDto.DocumentsLogoFile);
                    string documentName = Path.GetFileName(documentFile);
                    tenantDto.DocumentsLogo = documentName;
                }

                var tenant = await _tenantAppService.UpdateTenantAsync(tenantDto);
                return Json(new
                {
                    msg = "OK",
                    tenant = tenant
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
        public async Task<ActionResult> EditHostInfo()
        {
            var tenantInfo = await _tenantAppService.GetHostTenantInfoAsync();
            HostTenantDto tenantDto = ObjectMapper.Map<HostTenantDto>(tenantInfo);
            return View(tenantDto);
        }

        public IFormFile GetFileFromDirectory(string fileName)
        {
            if (fileName != null)
            {
                var dir = Path.Combine(_env.ContentRootPath, "wwwroot\\TenantLogos");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var filePath = Path.Combine(dir, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    return new FormFile(fileStream, 0, fileStream.Length, fileName, fileName);
                }
            }

            return null; // Or handle the case where the file doesn't exist
        }

        #region Private
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
            if (file != null)
            {
                var uniqueFileName = GetUniqueFileName(file.FileName);

                 uniqueFileName = !string.IsNullOrEmpty(uniqueFileName) ? uniqueFileName.Replace(" ", "_") : null;
               

                var dir = Path.Combine(_env.ContentRootPath, "wwwroot\\TenantLogos");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var filePath = Path.Combine(dir, uniqueFileName);
                await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

                return Path.Combine(@"\TenantLogos\", uniqueFileName);
            }
            else
            { return null; }
        }

        #endregion
    }
}
