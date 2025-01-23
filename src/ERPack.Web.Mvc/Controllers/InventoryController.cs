using Abp.AspNetCore.Mvc.Authorization;
using Abp.Logging;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Departments;
using ERPack.Inventory;
using ERPack.Inventory.Dto;
using ERPack.Materials;
using ERPack.Materials.Dto;
using ERPack.Stores;
using ERPack.Users;
using ERPack.Web.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Inventory)]
    public class InventoryController : ERPackControllerBase
    {
        private readonly IInventoryAppService _inventoryAppService;
        private readonly IMaterialAppService _materialAppService;
        private readonly IItemTypeAppService _itemTypeAppService;
        private readonly IDepartmentAppService _departmentAppService;
        private readonly IStoreAppService _storeAppService;

        public InventoryController(IInventoryAppService inventoryAppService,
            IMaterialAppService materialAppService, IItemTypeAppService itemTypeAppService,
            IDepartmentAppService departmentAppService, IUserAppService userAppService,
            IStoreAppService storeAppService)
        {
            _inventoryAppService = inventoryAppService;
            _materialAppService = materialAppService;
            _itemTypeAppService = itemTypeAppService;
            _departmentAppService = departmentAppService;
            _storeAppService = storeAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IssueInventory()
        {
            AddEditInventoryIssueModel model = new AddEditInventoryIssueModel();
            model.InventoryRequests = await _inventoryAppService.GetInventoryRequestsAsync();
            model.Materials = await _materialAppService.GetAllMaterialsAsync();
            model.ItemTypes = await _itemTypeAppService.GetItemTypesAsync();
            model.Departments = await _departmentAppService.GetDepartmentsAsync();
            model.Stores = await _storeAppService.GetAllStoresAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> IssueInventory(AddEditInventoryIssueModel input)
        {
            try
            {
                InventoryIssuedDto inventoryIssuedDto = ObjectMapper.Map<InventoryIssuedDto>(input);
                inventoryIssuedDto.TenantId = AbpSession.TenantId;

                var inventoryIssuedId = await _inventoryAppService.IssueInventoryAsync(inventoryIssuedDto);

                if (inventoryIssuedId != 0)
                {
                    foreach (var item in input.InventoryItems)
                    {
                        item.InventoryIssueId = inventoryIssuedId;
                        item.TenantId = AbpSession.TenantId;
                        await _inventoryAppService.AddInventoryItemAsync(item);

                       var fromStoreInventory = await  _materialAppService.GetMaterialInventoryByStoreAsync(item.MaterialId.Value, item.FromStoreId.Value);
                       var toStoreInventory = await _materialAppService.GetMaterialInventoryByStoreAsync(item.MaterialId.Value, item.ToStoreId.Value);

                        if(fromStoreInventory != null && toStoreInventory != null && fromStoreInventory.Id == toStoreInventory.Id)
                        {

                        }
                        else 
                        {
                            if(fromStoreInventory != null)
                            {
                                fromStoreInventory.Quantity = fromStoreInventory.Quantity - item.QtyTransferred;
                                await _materialAppService.UpdateMaterialInventoryAsync(fromStoreInventory);
                            }

                            if (toStoreInventory != null)
                            {
                                toStoreInventory.Quantity = toStoreInventory.Quantity + item.QtyTransferred;
                                await _materialAppService.UpdateMaterialInventoryAsync(toStoreInventory);
                            }
                            else
                            {
                                MaterialInventoryDto materialInventoryDto = new();
                                {
                                    materialInventoryDto.StoreId = item.ToStoreId.Value;
                                    materialInventoryDto.MaterialId = item.MaterialId.Value;
                                    materialInventoryDto.Quantity = item.QtyTransferred;
                                }
                                await _materialAppService.AddMaterialInventoryAsync(materialInventoryDto);
                            }
                        }
                       
                    }

                    return Json(new
                    {
                        msg = "OK",
                        id = inventoryIssuedId
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

        public async Task<JsonResult> GetInventoryItems(long inventoryId)
        {
            var inventoryItems = await _inventoryAppService.GetInventoryItemsAsync(inventoryId);
            if (inventoryItems != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = inventoryItems
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found Inventory Items");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        public async Task<JsonResult> GetInventoryRequest(long id)
        {
            var inventoryRequest = await _inventoryAppService.GetInventoryRequestAsync(id);
            if (inventoryRequest != null)
            {
                string jsonData = JsonConvert.SerializeObject(inventoryRequest);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });
            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found Inventory Items");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }
    }
}
