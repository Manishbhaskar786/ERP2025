using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DocumentFormat.OpenXml.Bibliography;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Departments;
using ERPack.Departments.Dto;
using ERPack.Users.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    public class DepartmentController : ERPackControllerBase
    {
        private readonly IDepartmentAppService _departmentappService;
        public DepartmentController(IDepartmentAppService departmentAppService)
        {
            _departmentappService = departmentAppService;
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Department)]
        public IActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Department)]
        public async Task<ActionResult> EditModal(int departmentId)
        {
            try
            {
                var departmentOutput = await _departmentappService.GetDepartmentByIdAsync(departmentId);
                return PartialView("_EditModal", departmentOutput);
            }
            catch (Exception ex)
            {
                return PartialView("_EditModal");
            }
        }
    }
}
