using Abp.AspNetCore.Mvc.Authorization;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Stores;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Stores)]
    public class StoresController : ERPackControllerBase
    {
        private readonly IStoreAppService _storeAppService;

        public StoresController(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> EditModal(int storeId)
        {
            var storeDto = await _storeAppService.GetAsync(storeId);
            return PartialView("_EditModal", storeDto);
        }
    }
}
