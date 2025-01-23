using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;

namespace ERPack.Web.Views.Shared.Components.SideBarMenu
{
    public class SideBarMenuViewComponent : ERPackViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IAbpSession _abpSession;

        public SideBarMenuViewComponent(
            IUserNavigationManager userNavigationManager,
            IAbpSession abpSession)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mainMenu = await _userNavigationManager.GetMenuAsync("MainMenu", _abpSession.ToUserIdentifier());

            // Filter the menu items based on the user's context
            var filteredMenu = FilterMenu(mainMenu);

            var model = new SideBarMenuViewModel
            {
                MainMenu = filteredMenu
            };

            //return View(model);
            //var model = new SideBarMenuViewModel
            //{
            //    MainMenu = await _userNavigationManager.GetMenuAsync("MainMenu", _abpSession.ToUserIdentifier())
            //};

            return View(model);
        }
        private UserMenu FilterMenu(UserMenu menu)
        {
            // Deep copy the menu to avoid modifying the original
            var filteredMenu = new UserMenu
            {
                Name = menu.Name,
                DisplayName = menu.DisplayName,
                Items = menu.Items
                    .Where(item => ShouldIncludeMenuItem(item))
                    .ToList()
            };

            return filteredMenu;
        }

        private bool ShouldIncludeMenuItem(UserMenuItem item)
        {
            // Determine whether to include a menu item based on the user's context
            if (_abpSession.MultiTenancySide == MultiTenancySides.Host)
            {
                if (item.Name == "Tenants")
                {
                    return true;
                }                

            }
            else if (_abpSession.MultiTenancySide == MultiTenancySides.Tenant)
            {
                return !item.Name.StartsWith("Host"); // Exclude host-specific items for tenant
            }

            return false; // Include by default
        }
    }
}
