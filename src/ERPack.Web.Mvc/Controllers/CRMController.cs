using Abp.AspNetCore.Mvc.Authorization;
using Abp.Logging;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Customers;
using ERPack.Departments;
using ERPack.Designs;
using ERPack.Enquiries;
using ERPack.Enquiries.Dto;
using ERPack.Estimates;
using ERPack.Estimates.Dto;
using ERPack.Helpers;
using ERPack.Materials;
using ERPack.Sessions;
using ERPack.Shared;
using ERPack.Web.Models.CRM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{

    public class CRMController : ERPackControllerBase
    {
        private readonly IDesignAppService _designAppService;
        private readonly ICustomerAppService _customerAppService;
        private readonly IEnquiryAppService _enquiryAppService;
        private readonly IItemTypeAppService _itemTypeAppService;
        private readonly IMaterialAppService _materialAppService;
        private readonly IUnitAppService _unitAppService;
        private readonly IEstimateAppService _estimateAppService;
        private readonly IHostEnvironment _env;
        private readonly IPdfHelper _pdfHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly ISessionAppService _sessionAppService;
        private readonly IFileUploadHelper _fileUploadHelper;

        public CRMController(IDesignAppService designAppService,
            ICustomerAppService customerAppService,
            IItemTypeAppService itemTypeAppService,
            IEnquiryAppService enquiryAppService,
            IMaterialAppService materialAppService,
            IUnitAppService unitAppService,
            IEstimateAppService estimateAppService,
            IHostEnvironment env,
            IPdfHelper pdfHelper,
            IEmailHelper emailHelper,
            ISessionAppService sessionAppService,
            IFileUploadHelper fileUploadHelper)
        {
            _designAppService = designAppService;
            _customerAppService = customerAppService;
            _itemTypeAppService = itemTypeAppService;
            _enquiryAppService = enquiryAppService;
            _materialAppService = materialAppService;
            _unitAppService = unitAppService;
            _estimateAppService = estimateAppService;
            _env = env;
            _pdfHelper = pdfHelper;
            _emailHelper = emailHelper;
            _sessionAppService = sessionAppService;
            _fileUploadHelper = fileUploadHelper;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Estimate
        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<ActionResult> Estimate(long estimateId)
        {
            AddEditEstimateModel model = new AddEditEstimateModel();
            if (estimateId != 0)
            {
                EstimateDto estimateDto = await _estimateAppService.GetAsync(estimateId);
                model = ObjectMapper.Map<AddEditEstimateModel>(estimateDto);
            }

            model.CompletedDesigns = await _designAppService.GetAllCompletedDesignsAsync();
            model.Materials = await _materialAppService.GetAllMaterialsAsync();
            model.Units = await _unitAppService.GetUnitsAsync();
            return View(model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        [HttpPost]
        public async Task<JsonResult> SaveEstimate(AddEditEstimateModel input)
        {
            try
            {
                EstimateDto estimateDto = ObjectMapper.Map<EstimateDto>(input);
                if (input.Id == 0)
                {
                    estimateDto.TenantId = AbpSession.TenantId;
                    estimateDto.Status = ERPackConsts.SentForApproval;

                    var estimateId = await _estimateAppService.CreateAsync(estimateDto);
                    if (estimateId != 0)
                    {
                        var design = await _designAppService.GetAsync(estimateDto.DesignId);

                        await _designAppService.UpdateStatusAsync(design.Id, ERPackConsts.SentForApproval);

                        var enquiry = await _enquiryAppService.GetAsync(design.EnquiryId.Value);

                        await _enquiryAppService.UpdateStatusAsync(enquiry.Id, ERPackConsts.SentForApproval);

                        foreach (var item in input.EstimateTasks)
                        {
                            item.EstimateId = estimateId;
                            item.TenantId = AbpSession.TenantId;
                            await _estimateAppService.CreateEstimateTaskAsync(item);
                        }
                        return Json(new
                        {
                            msg = "OK",
                            id = estimateId
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            msg = "ERROR",
                            id = 0
                        });
                    }
                }
                else
                {
                    var estimate = await _estimateAppService.UpdateAsync(estimateDto);

                    foreach (var item in input.EstimateTasks)
                    {
                        if (item.Id == 0 || item.Id == null)
                        {
                            item.EstimateId = estimate.Id;
                            await _estimateAppService.CreateEstimateTaskAsync(item);
                        }
                        else
                        {
                            await _estimateAppService.UpdateEstimateTaskAsync(item);
                        }
                    }
                    return Json(new
                    {
                        msg = "OK",
                        id = estimate.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error creating estimate", ex);
                return Json(new
                {
                    msg = "ERROR",
                    id = 0
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<JsonResult> GetEstimateTasks(long estimateId)
        {
            var estimateTasks = await _estimateAppService.GetEstimateTasksAsync(estimateId);
            if (estimateTasks != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = estimateTasks
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found Estimate Tasks");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        [HttpGet]
        public async Task<JsonResult> ApproveEstimate(long enquiryId, bool? isApproved)
        {
            var enquiryDto = await _enquiryAppService.GetAsync(enquiryId);
            enquiryDto.IsEstimateApproved = isApproved;
            if (isApproved == false)
            {
                enquiryDto.Status = ERPackConsts.Rejected;
            }
            else
            {
                enquiryDto.Status = ERPackConsts.Approved;
            }

            var enquiry = await _enquiryAppService.UpdateStatusAsync(enquiryId, enquiryDto.Status);

            if (enquiry != null)
            {
                var estimateDto = await _estimateAppService.GetEstimteByEnquiryIdAsync(enquiry.Id);
                if (isApproved == false)
                {
                    estimateDto.Status = ERPackConsts.Rejected;
                }
                else
                {
                    estimateDto.Status = ERPackConsts.Approved;
                }
                var estimate = await _estimateAppService.UpdateStatusAsync(estimateDto.Id, estimateDto.Status, isApproved);

                return Json(new
                {
                    msg = "OK",
                    data = estimate.EnquiryId
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found Enquiry");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult> ApproveEstimateByEmail(long enquiryId, bool? isApproved)
        {
            var enquiryDto = await _enquiryAppService.GetAsync(enquiryId);
            enquiryDto.IsEstimateApproved = isApproved;
            enquiryDto.Status = ERPackConsts.Approved;

            var enquiry = await _enquiryAppService.UpdateAsync(enquiryDto);

            if (enquiry != null)
            {
                var estimateDto = await _estimateAppService.GetEstimteByEnquiryIdAsync(enquiry.Id);
                estimateDto.IsEstimateApproved = isApproved;
                estimateDto.Status = ERPackConsts.Approved;
                await _estimateAppService.UpdateAsync(estimateDto);
            }
            return RedirectToAction("EstimateApproval");
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public IActionResult EstimatesList()
        {
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        [HttpGet]
        public async Task<FileResult> PdfEstimate(long estimateId)
        {
            var currentLoginInfo = await _sessionAppService.GetCurrentLoginInformations();
            byte[] workorderPdf = await _pdfHelper.GetEstimate(estimateId, currentLoginInfo);

            // Set the proper HTTP response content type
            FileContentResult fileContentResult =
                    File(workorderPdf, "application/pdf");
            return fileContentResult;
        }

        [HttpGet]
        public async Task<FileResult> PdfDesignJob(long designId) //function to generate PDF
        {
            var currentLoginInfo = await _sessionAppService.GetCurrentLoginInformations();
            byte[] workorderPdf = await _pdfHelper.GetDesignJobCard(designId, currentLoginInfo);

            // Set the proper HTTP response content type
            FileContentResult fileContentResult =
                    File(workorderPdf, "application/pdf");
            return fileContentResult;
        }


        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<JsonResult> EstimateApprovalEmail(long customerId, long enquiryId, string estimate)
        {

            var customer = await _customerAppService.GetAsync(customerId);
            var template = _emailHelper.GetEstimateTemplate(estimate, enquiryId);

            _emailHelper.SendEmail(customer.EmailAddress, "Approve Estimate", template);

            return Json(new
            {
                msg = "OK",
                data = "Estimate Sent for Approval !"
            });

        }

        #endregion

        #region Enquiry
        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<ActionResult> NewEnquiry(long enquiryId = 0, long customerId = 0)
        {
            ViewBag.Title = $"Create Enquiry";
            AddEditEnquiryModel model = new AddEditEnquiryModel();
            if (enquiryId > 0)
            {
                ViewBag.Title = $"Edit Enquiry";
                EnquiryDto enquiry = await _enquiryAppService.GetAsync(enquiryId);

                model = ObjectMapper.Map<AddEditEnquiryModel>(enquiry);
            }
            if (customerId != 0)
            {
                var customer = await _customerAppService.GetAsync(customerId);
                model.CustomerId = customerId;
                model.CustomerName = customer.Name;
            }
            if(model.ToolTypeId != 0)
            {
                model.Materials = model.ToolTypeId == 3
                    ? await _materialAppService.GetAllMaterialsAsync()
                    : await _materialAppService.GetAllByTypeAsync(model.ToolTypeId ?? 0);
            }
            
            return View(model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        [HttpPost]
        public async Task<JsonResult> SaveEnquiry(AddEditEnquiryModel input)
        {
            var enquiryDto = ObjectMapper.Map<EnquiryDto>(input);
            enquiryDto.TenantId = AbpSession.TenantId;

            var isNewEnquiry = input.Id == 0;

            if (isNewEnquiry)
            {
                enquiryDto.Status = ERPackConsts.Design;
                enquiryDto.StatusDatetime = DateTime.Now;
                var enquiryId = await CreateEnquiryAsync(enquiryDto, input.DesignImageDoc, input.EnquiryId.ToString());
                return Json(new { msg = enquiryId != 0 ? "OK" : "ERROR", id = enquiryId });
            }
            else
            {
                var enquiryId = await UpdateEnquiryAsync(enquiryDto, input.DesignImageDoc, input.EnquiryId.ToString());
                return Json(new { msg = enquiryId != 0 ? "OK" : "ERROR", id = enquiryId });
            }
        }


        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        [HttpPost]
        public async Task<IActionResult> DeleteEnquiryMaterial(int enquiryMaterialId)
        {
            if (enquiryMaterialId <= 0)
            {
                return BadRequest("Invalid material ID.");
            }

            try
            {
                await _enquiryAppService.DeleteDesignMaterialAsync(enquiryMaterialId);
                return Json(new { success = true, message = "Material deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<JsonResult> GetEnquiryById(long id)
        {
            var enquiry = await _enquiryAppService.GetAsync(id);
            if (enquiry == null)
            {
                return Json(new
                {
                    msg = "ERROR"
                });
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(enquiry);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });

            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public IActionResult EnquiryList()
        {
            return View();
        }

        #endregion

        #region Dropdowns Lists
        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<JsonResult> GetDesignNames(string query)
        {
            var designs = await _designAppService.GetDesignNamesAsync(query);
            if (designs != null)
            {
                string jsonData = JsonConvert.SerializeObject(designs);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });

            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found designs");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<JsonResult> GetBoardTypes(string query)
        {
            var boardTypes = await _designAppService.GetBoardTypesAsync(query);
            if (boardTypes != null)
            {
                string jsonData = JsonConvert.SerializeObject(boardTypes);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found boardtypes");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<JsonResult> GetToolConfiguraions()
        {
            var toolConfigurations = await _designAppService.GetToolConfigurationsAsync();
            if (toolConfigurations != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = toolConfigurations
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found designs");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<JsonResult> GetToolTypes()
        {
            var itemTypes = await _itemTypeAppService.GetItemTypesAsync();
            if (itemTypes != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = itemTypes
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found ItemTypes");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_CRM)]
        public async Task<JsonResult> GetEnquiryMaterials(int enquiryId)
        {
            var enquiryMaterials = await _enquiryAppService.GetEnquiryMaterialsAsync(enquiryId);

            if (enquiryMaterials != null)
            {
                string jsonData = JsonConvert.SerializeObject(enquiryMaterials);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found Enquiry Materials");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }
        #endregion


        #region "Save Operations"
        private async Task<long> CreateEnquiryAsync(EnquiryDto enquiryDto, IFormFile designImageDoc, string enquiryIdString)
        {
            var enquiryId = await _enquiryAppService.CreateAsync(enquiryDto);

            if (designImageDoc != null)
            {
                enquiryDto.DesignImage = await SaveFileAsync(enquiryId, enquiryIdString, designImageDoc);
                enquiryDto.Id = enquiryId;
                var updatedEnquiry = await _enquiryAppService.UpdateAsync(enquiryDto);
                enquiryId = updatedEnquiry.Id; // Update to save the file path
            }

            return enquiryId;
        }

        private async Task<long> UpdateEnquiryAsync(EnquiryDto enquiryDto, IFormFile designImageDoc, string enquiryIdString)
        {
            if (designImageDoc != null)
            {
                enquiryDto.DesignImage = await SaveFileAsync(enquiryDto.Id, enquiryIdString, designImageDoc);
            }

            var updatedEnquiry = await _enquiryAppService.UpdateAsync(enquiryDto);
            return updatedEnquiry.Id; // Update to save the file path
        }

        private async Task<string> SaveFileAsync(long enquiryId, string enquiryIdString, IFormFile designImageDoc)
        {
            var fileUpload = new FileUpload
            {
                Id = enquiryId.ToString(),
                Number = enquiryIdString,
                FolderName = ERPackConsts.Enquiry,
                File = designImageDoc
            };

            return await _fileUploadHelper.SaveFileAsync(fileUpload);
        }

        #endregion
    }
}
