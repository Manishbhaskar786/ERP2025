using Abp.AspNetCore.Mvc.Authorization;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.PurchaseIndents;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ERPack.PurchaseIndents.Dto;
using System.Collections.Generic;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_PurchaseIndent)]
    public class PurchaseIndentsController : ERPackControllerBase
    {
        private readonly IPurchaseIndentAppService _purchaseIndentAppService;

        public PurchaseIndentsController(IPurchaseIndentAppService purchaseIndentAppService)
        {
            _purchaseIndentAppService = purchaseIndentAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> CreatePurchaseIndents([FromBody] List<PurchaseIndentDto> purchaseIndents)
        {
            if (purchaseIndents != null && purchaseIndents.Count > 0)
            {
                foreach(var purchaseIndent in purchaseIndents)
                {
                    purchaseIndent.RequestedBy = AbpSession.UserId;
                    purchaseIndent.RequestedDate = DateTime.UtcNow;
                    await _purchaseIndentAppService.CreateAsync(purchaseIndent);
                }
            }
            return Json(new
            {
                msg = "OK",
                id = ""
            });
        }
    }
}
