using Abp.AspNetCore.Mvc.Authorization;
using Abp.Logging;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Departments;
using ERPack.Designs;
using ERPack.Designs.Dto;
using ERPack.Enquiries;
using ERPack.Estimates;
using ERPack.Helpers;
using ERPack.Inventory;
using ERPack.Inventory.Dto;
using ERPack.Materials;
using ERPack.Preferences;
using ERPack.Shared;
using ERPack.Users;
using ERPack.Web.Models.Production;
using ERPack.Workorders;
using ERPack.Workorders.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    public class ProductionController : ERPackControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IMaterialAppService _materialAppService;
        private readonly IEnquiryAppService _enquiryAppService;
        private readonly IDesignAppService _designAppService;
        private readonly IEstimateAppService _estimateAppService;
        private readonly IWorkorderAppService _workorderAppService;
        private readonly IUnitAppService _unitAppService;
        private readonly IProductionAppService _productionAppService;
        private readonly IInventoryAppService _inventoryAppService;
        private readonly IPreferenceAppService _preferenceAppService;
        private readonly IHostEnvironment _env;
        private readonly IPdfHelper _pdfHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly IFileUploadHelper _fileUploadHelper;

        public ProductionController(IUserAppService userAppService,
            IMaterialAppService materialAppService,
            IEnquiryAppService enquiryAppService,
            IDesignAppService designAppService,
            IEstimateAppService estimateAppService,
            IWorkorderAppService workorderAppService,
            IUnitAppService unitAppService,
            IProductionAppService productionAppService,
            IInventoryAppService inventoryAppService,
            IPreferenceAppService preferenceAppService,
            IHostEnvironment env,
            IPdfHelper pdfHelper,
            IEmailHelper emailHelper,
            IFileUploadHelper fileUploadHelper)
        {
            _userAppService = userAppService;
            _materialAppService = materialAppService;
            _enquiryAppService = enquiryAppService;
            _designAppService = designAppService;
            _estimateAppService = estimateAppService;
            _workorderAppService = workorderAppService;
            _unitAppService = unitAppService;
            _productionAppService = productionAppService;
            _inventoryAppService = inventoryAppService;
            _preferenceAppService = preferenceAppService;
            _env = env;
            _pdfHelper = pdfHelper;
            _emailHelper = emailHelper;
            _fileUploadHelper = fileUploadHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Workorder
        [AbpMvcAuthorize(PermissionNames.Pages_Workorder)]
        public async Task<ActionResult> AddWorkorder(long workorderId)
        {
            AddEditWorkorderModel model = new AddEditWorkorderModel();
            if (workorderId != 0)
            {
                var workorderDto = await _workorderAppService.GetAsync(workorderId);
                model = ObjectMapper.Map<AddEditWorkorderModel>(workorderDto);
                model.Estimates = await _estimateAppService.GetAllEstimatesAsync();
            }
            else
            {
                model.Estimates = await _estimateAppService.GetApprovedEstimatesAsync();
            }
            model.Materials = await _materialAppService.GetAllMaterialsAsync();
            model.Units = await _unitAppService.GetUnitsAsync();
            
            return View(model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Workorder)]
        public IActionResult WorkorderList()
        {
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Workorder)]
        [HttpPost]
        public async Task<JsonResult> SaveWorkorder(AddEditWorkorderModel input)
        {
            try
            {
                WorkorderDto workorderDto = ObjectMapper.Map<WorkorderDto>(input);
                workorderDto.TenantId = AbpSession.TenantId;
                workorderDto.Status = ERPackConsts.InProgress;

                if (input.WorkorderTasks.Count > 1)
                {
                    workorderDto.TaskIssueDate = input.WorkorderTasks
                    .Select(r => r.TaskIssueDate == null ? (DateTime?)null : r.TaskIssueDate)
                    .Max();

                    workorderDto.TaskIssueCompleteDate = input.WorkorderTasks
                    .Select(r => r.TaskIssueCompleteDate == null ? (DateTime?)null : r.TaskIssueCompleteDate)
                    .Max();

                    workorderDto.TaskIssueActualCompleteDate = input.WorkorderTasks
                    .Select(r => r.TaskIssueActualCompleteDate == null ? (DateTime?)null : r.TaskIssueActualCompleteDate)
                    .Max();
                }

                var workOrderId = await _workorderAppService.CreateAsync(workorderDto);

                if (workOrderId != 0)
                {
                    var estimate = await _estimateAppService.GetAsync(input.EstimateId);

                    await _enquiryAppService.UpdateStatusAsync(estimate.EnquiryId, ERPackConsts.Production);

                    await _estimateAppService.UpdateStatusAsync(input.EstimateId, ERPackConsts.Production, true);

                    foreach (var item in input.WorkorderSubTasks)
                    {
                        item.TenantId = AbpSession.TenantId;
                        item.WorkorderId = workOrderId;
                        await _workorderAppService.CreateWorkorderSubTaskAsync(item);
                    }

                    foreach (var item in input.WorkorderTasks)
                    {
                        var workorderSubTask = await _workorderAppService.GetWorkorderTaskByDepartmentAsync(workOrderId, item.DepartmentId);
                        item.WorkorderSubTaskId = workorderSubTask.Id;
                        item.WorkorderId = workOrderId;
                        item.Status = ERPackConsts.InProgress;
                        item.TenantId = AbpSession.TenantId;
                        await _workorderAppService.CreateWorkorderTaskAsync(item);

                        if (item.UserId != null)
                        {
                            var user = await _userAppService.GetByIdAsync(item.UserId.Value);

                            _emailHelper.SendEmail(user.EmailAddress, "New Task", "New Task is assigned to you.");
                        }
                    }

                    return Json(new
                    {
                        msg = "OK",
                        id = workOrderId
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
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error creating workorder", ex);
                return Json(new
                {
                    msg = "ERROR",
                    id = 0
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Workorder)]
        public async Task<JsonResult> GetWorkorderTasks(long workorderId)
        {
            var workorderTasks = await _workorderAppService.GetWorkorderTasksAsync(workorderId);
            if (workorderTasks != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = workorderTasks
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found Workorder Tasks");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Workorder)]
        public async Task<JsonResult> GetWorkorderTask(long workorderTaskId)
        {
            var workorderTasks = await _workorderAppService.GetWorkorderTaskAsync(workorderTaskId);
            if (workorderTasks != null)
            {
                string jsonData = JsonConvert.SerializeObject(workorderTasks);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found Workorder Tasks");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Workorder)]
        public async Task<JsonResult> GetCompletedWorkorderTasks()
        {
            var workorderTasks = await _workorderAppService.GetWorkorderTasksByStatusAsync(ERPackConsts.Completed);
            if (workorderTasks != null)
            {
                string jsonData = JsonConvert.SerializeObject(workorderTasks);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found Workorder Tasks");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Workorder)]
        [HttpGet]
        public async Task<ActionResult> SetSubTaskStatus(long workorderTaskId, string status)
        {
            var isUpdated = await _productionAppService.UpdateWorkorderTaskStatusAsync(workorderTaskId, status);
            if (isUpdated)
            {
                return Json(new
                {
                    msg = "OK",
                    data = "StatusUpdated"
                });
            }
            else
            {
                return Json(new
                {
                    msg = "FAIL",
                    data = "Error in Saving !"
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Workorder)]
        [HttpPost]
        public async Task<ActionResult> RequestStock([FromBody] List<InventoryRequestDto> inventoryRequests)
        {
            try
            {
                bool isUpdated = false;
                foreach (var item in inventoryRequests)
                {
                    var inventoryRequestId = await _preferenceAppService.GetByNameAsync("InventoryReqId");
                    item.RequestFromUserId = AbpSession.UserId.Value;
                    item.InventoryRequestId = inventoryRequestId;
                    await _inventoryAppService.InventoryRequestAsync(item);
                    isUpdated = true;
                }
                if (isUpdated)
                {
                    return Json(new
                    {
                        msg = "OK",
                        data = "StockRequested"
                    });
                }
                else
                {
                    return Json(new
                    {
                        msg = "FAIL",
                        data = "Error"
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error in saving Request Stock", ex);
                return Json(new
                {
                    msg = "ERROR"
                });
            }

        }

        [AbpMvcAuthorize(PermissionNames.Pages_Workorder)]
        [HttpGet]
        public async Task<FileResult> PdfWorkorder(long workorderId)
        {
            byte[] workorderPdf = await _pdfHelper.GetWorkorder(workorderId, "");

            // Set the proper HTTP response content type
            FileContentResult fileContentResult =
                    File(workorderPdf, "application/pdf");
            return fileContentResult;
        }

        #endregion

        #region Design
        public async Task<ActionResult> Design()
        {
            AddEditDesignModel model = new()
            {
                Materials = await _materialAppService.GetAllMaterialsAsync()
            };
            return View(model);
        }

        public async Task<JsonResult> GetDesignById(long id)
        {
            var design = await _designAppService.GetAsync(id);
            if (design == null)
            {
                return Json(new
                {
                    msg = "ERROR"
                });
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(design);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });

            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDesignMaterial(int designMaterialId)
        {
            if (designMaterialId <= 0)
            {
                return BadRequest("Invalid material ID.");
            }

            try
            {
              
                await _designAppService.DeleteDesignMaterialAsync(designMaterialId);
                return Json(new { success = true, message = "Material deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public async Task<JsonResult> GetDesignMaterials(long id)
        {
            var designMaterials = await _designAppService.GetDesignMaterialsAsync(id);
            if (designMaterials == null)
            {
                return Json(new
                {
                    msg = "ERROR"
                });
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(designMaterials);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });

            }
        }

        #endregion

        #region "Save Operations"

        [AbpMvcAuthorize(PermissionNames.Pages_Design)]
        [HttpPost]
        public async Task<JsonResult> SaveDesign(AddEditDesignModel input)
        {
            var designDto = ObjectMapper.Map<DesignDto>(input);
            designDto.TenantId = AbpSession.TenantId;

            var isNewDesign = input.Id ?? 0;

            if (isNewDesign == 0)
            {
                designDto.Status = ERPackConsts.Estimate;
                designDto.StatusDatetime = DateTime.Now;
                designDto.CompletionDatetime = DateTime.Now;
                designDto.DesignId = await _preferenceAppService.GetByNameAsync("DesignId"); // DESIGN NUMBER DES0001
                var updatedEnquiry = await _enquiryAppService.UpdateStatusAsync(designDto.EnquiryId.Value, ERPackConsts.Estimate);

                var designId = await CreateDesignAsync(designDto, input.DesignImageDoc, designDto.DesignId.ToString());
                return Json(new { msg = designId != 0 ? "OK" : "ERROR", id = designId });
            }
            else
            {
                designDto.CompletionDatetime = DateTime.Now;
                var designId = await UpdateDesignAsync(designDto, input.DesignImageDoc, input.DesignId.ToString());
                return Json(new { msg = designId != 0 ? "OK" : "ERROR", id = designId });
            }
        }
        private async Task<long> CreateDesignAsync(DesignDto designDto, IFormFile designImageDoc, string designIdString)
        {
            var designId = await _designAppService.CreateAsync(designDto);

            if (designImageDoc != null)
            {
                designDto.ReportDoc = await SaveFileAsync(designId, designIdString, designImageDoc);
                designDto.Id = (int)designId;
                var updatedDesignData = await _designAppService.UpdateAsync(designDto);
                designId = updatedDesignData.Id; // Update to save the file path
            }

            return designId;
        }

        private async Task<long> UpdateDesignAsync(DesignDto designDto, IFormFile designImageDoc, string designIdString)
        {
            if (designImageDoc != null)
            {
                designDto.ReportDoc = await SaveFileAsync(designDto.Id, designIdString, designImageDoc);
            }

            var updatedDesignData = await _designAppService.UpdateAsync(designDto);
            return updatedDesignData.Id; // Update to save the file path
        }

        private async Task<string> SaveFileAsync(long designId, string designIdString, IFormFile designImageDoc)
        {
            var fileUpload = new FileUpload
            {
                Id = designId.ToString(),
                Number = designIdString,
                FolderName = ERPackConsts.Design,
                File = designImageDoc
            };

            return await _fileUploadHelper.SaveFileAsync(fileUpload);
        }

        #endregion
    }
}
