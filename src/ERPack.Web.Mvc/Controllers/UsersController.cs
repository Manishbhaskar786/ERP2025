using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using ERPack.Authorization;
using ERPack.Controllers;
using ERPack.Users;
using ERPack.Web.Models.Users;
using ERPack.Departments;
using ERPack.Departments.Dto;
using ERPack.Users.Dto;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Hosting;
using ERPack.Authorization.Users;
using ERPack.Common;

namespace ERPack.Web.Controllers
{
    public class UsersController : ERPackControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IDepartmentAppService _departmentAppService;
        private readonly ICountryMasterAppService _countryMasterAppService;
        private readonly IStateMasterAppService _stateMasterAppService;

        public UsersController(IUserAppService userAppService,
            IDepartmentAppService departmentAppService, ICountryMasterAppService countryMasterAppService, IStateMasterAppService stateMasterAppService)
        {
            _userAppService = userAppService;
            _departmentAppService = departmentAppService;
            _countryMasterAppService = countryMasterAppService;
            _stateMasterAppService = stateMasterAppService;
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Users)]
        public async Task<ActionResult> Index()
        {
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new UserListViewModel
            {
                Roles = roles
            };
            return View(model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Users)]
        public async Task<ActionResult> CreateUser(long userId = 0)
        {
            AddEditUserModel model = new AddEditUserModel();
            if (userId > 0)
            {
                var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
                model = ObjectMapper.Map<AddEditUserModel>(user);
            }
            var roles = (await _userAppService.GetRoles()).Items;
            var departments = await _departmentAppService.GetDepartmentsAsync();
            var Countries = await _countryMasterAppService.GetCountriesAsync(0);
            var States = await _stateMasterAppService.GetStatesAsync(0);

            model.Roles = roles;
            model.Departments = departments;
            model.Countries = Countries;
            model.States = States;

            return View(model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Users)]
        [HttpPost]
        public async Task<JsonResult> CreateUser(CreateUserDto createUserDto)
        {
            var user = await _userAppService.CreateAsync(createUserDto);
            if (user == null)
            {
                return Json(new
                {
                    msg = "ERROR"
                });
            }
            else
            {
                return Json(new
                {
                    msg = "OK",
                    id = user.Id
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Users)]
        public async Task<JsonResult> GetUsersByDepartment(int departmentId)
        {
            var users = await _userAppService.GetUsersByDepartmentAsync(departmentId);
            if (users == null)
            {
                return Json(new
                {
                    msg = "ERROR"
                });
            }
            else
            {
                return Json(new
                {
                    msg = "OK",
                    users = users
                });
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Users)]
        public async Task<JsonResult> GetDesignUsers()
        {
            var department = await _departmentAppService.GetDepartmentByNameAsync("Design");
            if (department == null)
            {
                return Json(new
                {
                    msg = "ERROR"
                });
            }
            else
            {
                var users = await _userAppService.GetUsersByDepartmentAsync(department.Id);

                if (users != null)
                {
                    return Json(new
                    {
                        msg = "OK",
                        data = users
                    });
                }
                else
                {
                    return Json(new
                    {
                        msg = "ERROR"
                    });
                }
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Users)]
        public async Task<ActionResult> EditModal(long userId)
        {
            var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new EditUserModalViewModel
            {
                User = user,
                Roles = roles
            };
            return PartialView("_EditModal", model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Users)]
        public async Task<ActionResult> ViewUser(long userId)
        {
            var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
            if (user.Department == null)
            {
                user.Department = new Department(); 
            }

            var department = await _departmentAppService.GetDepartmentByIdAsync(user.DepartmentId);
            if(department != null) {
                user.Department.DeptName = department.DeptName;
            }
            else {
                user.Department.DeptName = string.Empty;
            }

            return View(user);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
