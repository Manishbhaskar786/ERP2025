using Abp.AspNetCore.Mvc.Authorization;
using Abp.Logging;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Materials;
using ERPack.Materials.Dto;
using ERPack.PurchaseOrders;
using ERPack.PurchaseReceives.Dto;
using ERPack.PurchaseRecieves;
using ERPack.Stores;
using ERPack.Web.Models.PurchaseReceive;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_PurchaseReceive)]
    public class PurchaseReceivesController : ERPackControllerBase
    {
        private readonly IPurchaseReceiveAppService _purchaseReceiveAppService;
        private readonly IPurchaseOrderAppService _purchaseOrderAppService;
        private readonly IMaterialAppService _materialAppService;
        private readonly IStoreAppService _storeAppService;

        public PurchaseReceivesController(IPurchaseReceiveAppService purchaseReceiveAppService,
            IPurchaseOrderAppService purchaseOrderAppService,
            IMaterialAppService materialAppService,
            IStoreAppService storeAppService)
        {
            _purchaseReceiveAppService = purchaseReceiveAppService;
            _purchaseOrderAppService = purchaseOrderAppService;
            _storeAppService = storeAppService;
            _materialAppService = materialAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddPurchaseReceive()
        {
            AddEditPurchaseReceiveModel model = new AddEditPurchaseReceiveModel
            {
                Stores = await _storeAppService.GetAllStoresAsync(),
                PurchaseOrders = await _purchaseOrderAppService.GetAllPurchaseOrdersAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddPurchaseReceive(AddEditPurchaseReceiveModel input)
        {
            try
            {
                PurchaseReceiveDto purchaseReceiveDto = ObjectMapper.Map<PurchaseReceiveDto>(input);
                purchaseReceiveDto.TenantId = AbpSession.TenantId;

                var purchaseReceiveId = await _purchaseReceiveAppService.CreateAsync(purchaseReceiveDto);

                if (purchaseReceiveId != 0)
                {
                    foreach (var item in input.PurchaseReceiveItems)
                    {
                        item.PurchaseReceiveId = purchaseReceiveId;
                        item.TenantId = AbpSession.TenantId;
                        await _purchaseReceiveAppService.CreatePurchaseReceiveItemAsync(item);

                        var storeInventory = await _materialAppService.GetMaterialInventoryByStoreAsync(item.MaterialId.Value, item.StoreId.Value);
                        if (storeInventory != null)
                        {
                            storeInventory.Quantity = storeInventory.Quantity + item.QuantityReceived.Value;
                            await _materialAppService.UpdateMaterialInventoryAsync(storeInventory);
                        }
                        else
                        {
                            MaterialInventoryDto materialInventoryDto = new MaterialInventoryDto
                            {
                                StoreId = item.StoreId.Value,
                                MaterialId = item.MaterialId.Value,
                                Quantity = item.QuantityReceived.Value
                            };

                            await _materialAppService.AddMaterialInventoryAsync(materialInventoryDto);
                        }
                    }

                    return Json(new
                    {
                        msg = "OK",
                        id = purchaseReceiveId
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
                Logger.Log(LogSeverity.Error, "Error in issuing inventory", ex);
                return Json(new
                {
                    msg = "ERROR",
                    id = 0
                });
            }
        }

        public async Task<JsonResult> GetPurchaseReceiveItems(int purchaseReceiveId)
        {
            var purchaseReceiveItems = await _purchaseReceiveAppService.GetPurchaseReceiveItemsAsync(purchaseReceiveId);
            if (purchaseReceiveItems != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = purchaseReceiveItems
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found PurchaseReceiveItems");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        public async Task<JsonResult> GetPurchaseOrderItemsByPOCode(string poCode)
        {
            var purchaseOrderItems = await _purchaseOrderAppService.GetAllByPOCodeAsync(poCode);
            if (purchaseOrderItems != null)
            {
                string jsonData = JsonConvert.SerializeObject(purchaseOrderItems);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found PurchaseOrderItems");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }
    }
}
