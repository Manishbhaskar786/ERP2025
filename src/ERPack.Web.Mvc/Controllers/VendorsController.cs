using Abp.AspNetCore.Mvc.Authorization;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Vendors;
using ERPack.Vendors.Dto;
using ERPack.Web.Models.Vendors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Vendors)]
    public class VendorsController : ERPackControllerBase
    {
        private readonly IVendorAppService _vendorAppService;

        public VendorsController(IVendorAppService vendorAppService)
        {
            _vendorAppService = vendorAppService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddVendor(int vendorId = 0)
        {
            AddEditVendorModel model = new AddEditVendorModel();
            if (vendorId > 0)
            {
                VendorDto vendor = await _vendorAppService.GetAsync(vendorId);
                model = ObjectMapper.Map<AddEditVendorModel>(vendor);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddVendor(VendorDto vendorDto)
        {
            vendorDto.TenantId = AbpSession.TenantId;

            int vendorId = 0 ;
            if (vendorDto.Id == 0)
            {
                vendorId = await _vendorAppService.CreateAsync(vendorDto);
            }
            else
            {
                var vendor = await _vendorAppService.UpdateAsync(vendorDto);
                vendorId = vendor.Id;
            }

            if (vendorId == 0)
            {
                return Json(new
                {
                    msg = "ERROR",
                    id = 0
                });
            }
            else
            {
                return Json(new
                {
                    msg = "OK",
                    id = vendorId
                });
            }
        }
    }
}
