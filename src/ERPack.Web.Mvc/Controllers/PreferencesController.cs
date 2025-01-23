using Abp.AspNetCore.Mvc.Authorization;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Preferences;
using ERPack.Preferences.Dto;
using ERPack.Web.Models.Preferences;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Preferences)]
    public class PreferencesController : ERPackControllerBase
    {
        private readonly IPreferenceAppService _preferenceAppService;

        public PreferencesController(IPreferenceAppService preferenceAppService)
        {
            _preferenceAppService = preferenceAppService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddPreference(int preferenceId = 0)
        {
            AddEditPreferenceModel model = new AddEditPreferenceModel();
            if (preferenceId > 0)
            {
                PreferenceDto preference = await _preferenceAppService.GetAsync(preferenceId);
                model = ObjectMapper.Map<AddEditPreferenceModel>(preference);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddPreference(PreferenceDto preferenceDto)
        {
            preferenceDto.TenantId = AbpSession.TenantId;

            if (preferenceDto.Id == 0)
            {
                var preferenceId = await _preferenceAppService.CreateAsync(preferenceDto);

                if (preferenceId == -1)
                {
                    return Json(new
                    {
                        msg = "ERROR",
                        id = 0
                    });
                }
                else if (preferenceId == 0)
                {
                    return Json(new
                    {
                        msg = "EXISTS",
                        id = 0
                    });
                }
                else
                {
                    return Json(new
                    {
                        msg = "OK",
                        id = preferenceId
                    });
                }
            }
            else
            {
                var preference = await _preferenceAppService.UpdateAsync(preferenceDto);

                var preferenceId = preference.Id;

                if (preferenceId == -1)
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
                        id = preferenceId
                    });
                }
            }
        }
    }
}
