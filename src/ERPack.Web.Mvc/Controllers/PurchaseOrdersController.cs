using Abp.AspNetCore.Mvc.Authorization;
using Abp.Logging;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Departments;
using ERPack.Materials;
using ERPack.PurchaseOrders;
using ERPack.PurchaseOrders.Dto;
using ERPack.Vendors;
using ERPack.Web.Models.PurchaseOrder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_PurchaseOrder)]
    public class PurchaseOrdersController : ERPackControllerBase
    {
        private readonly IPurchaseOrderAppService _purchaseOrderAppService;
        private readonly IVendorAppService _vendorAppService;
        private readonly IItemTypeAppService _itemTypeAppService;
        private readonly IUnitAppService _unitAppService;
        private readonly IMaterialAppService _materiaLAppService;

        public PurchaseOrdersController(IPurchaseOrderAppService purchaseOrderAppService,
            IVendorAppService vendorAppService,
            IItemTypeAppService itemTypeAppService,
            IUnitAppService unitAppService,
            IMaterialAppService materiaLAppService)
        {
            _purchaseOrderAppService = purchaseOrderAppService;
            _vendorAppService = vendorAppService;
            _itemTypeAppService = itemTypeAppService;
            _unitAppService = unitAppService;
            _materiaLAppService = materiaLAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddPurchaseOrder()
        {
            AddEditPurchaseOrderModel model = new AddEditPurchaseOrderModel
            {
                Vendors = await _vendorAppService.GetAllVendorsAsync(),
                ItemTypes = await _itemTypeAppService.GetItemTypesAsync(),
                Materials = await _materiaLAppService.GetAllMaterialsAsync(),
                Units = await _unitAppService.GetUnitsAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddPurchaseOrder(AddEditPurchaseOrderModel input)
        {
            try
            {
                if(input.Id == null)
                {
                    PurchaseOrderDto purchaseOrderDto = ObjectMapper.Map<PurchaseOrderDto>(input);
                    purchaseOrderDto.PurchaseItem = input.PurchaseOrderItems.Count;
                    purchaseOrderDto.TenantId = AbpSession.TenantId;

                    var purchaseOrderId = await _purchaseOrderAppService.CreateAsync(purchaseOrderDto);

                    if (purchaseOrderId != 0)
                    {
                        foreach (var item in input.PurchaseOrderItems)
                        {
                            item.PurchaseOrderId = purchaseOrderId;
                            item.TenantId = AbpSession.TenantId;
                            await _purchaseOrderAppService.CreatePurchaseOrderItemAsync(item);
                        }

                        return Json(new
                        {
                            msg = "OK",
                            id = purchaseOrderId
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
                    PurchaseOrderDto purchaseOrderDto = ObjectMapper.Map<PurchaseOrderDto>(input);
                    purchaseOrderDto.PurchaseItem = input.PurchaseOrderItems.Count;
                    purchaseOrderDto.TenantId = AbpSession.TenantId;

                    var purchaseOrderId = await _purchaseOrderAppService.UpdateAsync(purchaseOrderDto);

                    if (purchaseOrderId != 0)
                    {
                        foreach (var item in input.PurchaseOrderItems)
                        {
                            item.PurchaseOrderId = purchaseOrderId;
                            item.TenantId = AbpSession.TenantId;
                            await _purchaseOrderAppService.updatePurchaseOrderItemAsync(item);
                        }

                        return Json(new
                        {
                            msg = "OK",
                            id = purchaseOrderId
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

        public async Task<ActionResult> EditPurchaseOrder(int purchaseOrderId)
        {
            AddEditPurchaseOrderModel model = new AddEditPurchaseOrderModel
            {
                Vendors = await _vendorAppService.GetAllVendorsAsync(),
                ItemTypes = await _itemTypeAppService.GetItemTypesAsync(),
                Materials = await _materiaLAppService.GetAllMaterialsAsync(),
                Units = await _unitAppService.GetUnitsAsync()
            };

            var existingPurchaseorder = await _purchaseOrderAppService.GetByIdAsync(purchaseOrderId);
            if(existingPurchaseorder != null)
            {
                model.Id = existingPurchaseorder.Id;
                model.TenantId = existingPurchaseorder.TenantId;
                model.VendorId = existingPurchaseorder.VendorId;
                model.POCode = existingPurchaseorder.POCode;
                model.PurchaseDate = existingPurchaseorder.CreationTime;
                model.PurchaseOrderItems = await _purchaseOrderAppService.GetPurchaseOrderItemsAsync(existingPurchaseorder.Id);
            }
            

            return View(model);
        }

        public async Task<JsonResult> GetPurchaseOrderItems(int purchaseorderId)
        {
            var purchaseOrderItems = await _purchaseOrderAppService.GetPurchaseOrderItemsAsync(purchaseorderId);
            if (purchaseOrderItems != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = purchaseOrderItems
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
