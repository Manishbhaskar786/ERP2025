using Abp.AspNetCore.Mvc.Authorization;
using Abp.Logging;
using ERPack.Authorization;
using ERPack.Categories;
using ERPack.Controllers;
using ERPack.Departments;
using ERPack.Materials;
using ERPack.Materials.Dto;
using ERPack.Web.Models.Materials;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Materials)]
    public class MaterialsController : ERPackControllerBase
    {
        private readonly IMaterialAppService _materialAppService;
        private readonly IDepartmentAppService _departmentAppService;
        private readonly IItemTypeAppService _itemTypeAppService;
        private readonly IUnitAppService _unitAppService;
        private readonly ICategoryAppService _categoryAppService;

        public MaterialsController(IMaterialAppService materialAppService,
            IDepartmentAppService departmentAppService,
            IItemTypeAppService itemTypeAppService,
            IUnitAppService unitAppService,
            ICategoryAppService categoryAppService)
        {
            _materialAppService = materialAppService;
            _departmentAppService = departmentAppService;
            _itemTypeAppService = itemTypeAppService;
            _unitAppService = unitAppService;
            _categoryAppService = categoryAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AddMaterial(long materialId = 0)
        {
            AddEditMaterialModel model = new AddEditMaterialModel();
            if (materialId != 0)
            {
                MaterialDto material = await _materialAppService.GetAsync(materialId);
                model = ObjectMapper.Map<AddEditMaterialModel>(material);
            }

            var departments = await _departmentAppService.GetDepartmentsAsync();
            var itemTypes = await _itemTypeAppService.GetItemTypesAsync();
            var units = await _unitAppService.GetUnitsAsync();
            var categories = await _categoryAppService.GetAllAsync();

            model.Departments = departments;
            model.ItemTypes = itemTypes;
            model.Units = units;
            model.Categories = categories;

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddMaterial(MaterialDto materialDto)
        {
            materialDto.TenantId = AbpSession.TenantId;

            if (materialDto.Id == 0)
            {
                var materialId = await _materialAppService.CreateAsync(materialDto);

                if (materialId == 0)
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
                        id = materialId
                    });
                }
            }
            else
            {
                var material = await _materialAppService.UpdateAsync(materialDto);

                return Json(new
                {
                    msg = "OK",
                    id = material.Id
                });

            }
        }
        //Controller Method to fetch all materials from table
        public async Task<JsonResult> GetAllMaterials()
        {

            var materials = await _materialAppService.GetAllMaterialsAsync();
            if (materials != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = materials
                });

            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found material");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }
        public async Task<JsonResult> GetMaterialById(long id)
        {
            var material = await _materialAppService.GetAsync(id);
            if (material != null)
            {
                string jsonData = JsonConvert.SerializeObject(material);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });

            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found material");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        public async Task<JsonResult> GetMaterialByType(int typeId)
        {
            
            var materials = await _materialAppService.GetAllByTypeAsync(typeId);
            if (materials != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = materials
                });

            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found material");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        public async Task<JsonResult> GetMaterialInventoryById(int materialId, int storeId)
        {
            var materialInventory = await _materialAppService.GetMaterialInventoryByStoreAsync(materialId, storeId);
            if (materialInventory != null)
            {
                return Json(new
                {
                    msg = "OK",
                    data = materialInventory
                });

            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found material");
                return Json(new
                {
                    msg = "ERROR",
                    data = ""
                });
            }
        }
    }
}
