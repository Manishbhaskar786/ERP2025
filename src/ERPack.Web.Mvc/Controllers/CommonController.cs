using Abp.Logging;
using ERPack.Categories;
using ERPack.Categories.Dto;
using ERPack.Controllers;
using ERPack.Departments;
using ERPack.Departments.Dto;
using ERPack.Helpers;
using ERPack.Preferences;
using ERPack.Units.Dto;
using ERPack.Web.Models.Common.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    public class CommonController : ERPackControllerBase
    {
        private readonly IDepartmentAppService _departmentAppService;
        private readonly IPreferenceAppService _preferenceAppService;
        private readonly IUnitAppService _unitAppService;
        private readonly IPdfHelper _pdfHelper;
        private readonly IExcelHelper _excelHelper;
        private readonly IHostEnvironment _env;
        private readonly ICategoryAppService _categoryAppService;


        public CommonController(IDepartmentAppService departmentAppService,
            IPreferenceAppService preferenceAppService,
            IUnitAppService unitAppService,
            IPdfHelper pdfHelper,
            IExcelHelper excelHelper,
        IHostEnvironment env,
        ICategoryAppService categoryAppService
        )
        {
            _departmentAppService = departmentAppService;
            _preferenceAppService = preferenceAppService;
            _unitAppService = unitAppService;
            _pdfHelper = pdfHelper;
            _excelHelper = excelHelper;
            _categoryAppService = categoryAppService;
            _env = env;
        }

        [HttpPost]
        public async Task<JsonResult> AddDepartment(string name)
        {
            DepartmentDto departmentDto = new();
            {
                departmentDto.DeptName = name;
                departmentDto.TenantId = AbpSession.TenantId;
                departmentDto.CreatorUserId = AbpSession.UserId;
            }
            var department = await _departmentAppService.CreateAsync(departmentDto);
            if (department.Id == 0)
            {
                Logger.Log(LogSeverity.Warn, "Not able to Create Department");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
            else
            {
                Logger.Log(LogSeverity.Info, string.Format("Added New Department with Id: {0}", department.Id));
                return Json(new
                {
                    msg = "OK",
                    id = department.Id
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddUnit(string name)
        {
            UnitDto unitDto = new();
            {
                unitDto.UnitName = name;
                unitDto.TenantId = AbpSession.TenantId;
                unitDto.CreatorUserId = AbpSession.UserId;
            }

            var unitId = await _unitAppService.CreateUnitAsync(unitDto);
            if (unitId == 0)
            {
                Logger.Log(LogSeverity.Warn, "Not able to Create Unit");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
            else
            {
                Logger.Log(LogSeverity.Info, string.Format("Added New Unit with Id: {0}", unitId));
                return Json(new
                {
                    msg = "OK",
                    id = unitId
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddCategory(string name)
        {
            CategoryDto categoryDto = new();
            {
                categoryDto.CategoryName = name;
                categoryDto.TenantId = AbpSession.TenantId;
                categoryDto.CreatorUserId = AbpSession.UserId;
            }
           
            (var categoryId, var errMsg) = await _categoryAppService.CreateCategoryAsync(categoryDto);
            if (categoryId == 0)
            {
                Logger.Log(LogSeverity.Warn, "Not able to Create Category");
                return Json(new
                {
                    msg = errMsg
                });
            }
            else
            {
                Logger.Log(LogSeverity.Info, string.Format("Added New Category with Id: {0}", categoryId));
                return Json(new
                {
                    msg = "OK",
                    id = categoryId
                });
            }
        }


        public async Task<JsonResult> GetIdByPreference(string idType, string name)
        {
            var Id = await _preferenceAppService.GetByNameAsync(idType, name);
            if (!string.IsNullOrEmpty(Id))
            {
                return Json(new
                {
                    msg = "OK",
                    id = Id
                });

            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found id preference");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        public async Task<JsonResult> GetDepartments()
        {
            var departments = await _departmentAppService.GetDepartmentsAsync();
            if (departments != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = departments
                });

            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found departments");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GeneratePdf([FromBody] CreatePdfModel createPdf)
        {
            byte[] pdf = _pdfHelper.ExportTable(createPdf.Name, createPdf.Html);

            var dir = Path.Combine(_env.ContentRootPath, "wwwroot/Documents");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var fileName = createPdf.Name + ".pdf";

            var filePath = Path.Combine(dir, fileName);

            System.IO.File.WriteAllBytes(filePath, pdf);

            return Json(new
            {
                msg = "OK",
                data = "/Documents/" + fileName
            });
        }

        [HttpPost]
        public async Task<JsonResult> GenerateExcel([FromBody] CreatePdfModel createExcel)
        {
            var dtData = _excelHelper.ExportToDataTable(createExcel.Html);

            var dir = Path.Combine(_env.ContentRootPath, "wwwroot/Documents");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var fileName = createExcel.Name + ".xlsx";

            var filePath = Path.Combine(dir, fileName);

            _excelHelper.CreateExcelDocument(dtData, filePath);

            return Json(new
            {
                msg = "OK",
                data = "/Documents/" + fileName
            });
        }
    }
}