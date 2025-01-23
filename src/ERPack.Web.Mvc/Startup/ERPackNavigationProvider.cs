using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using ERPack.Authorization;

namespace ERPack.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class ERPackNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("HomePage"),
                        url: "",
                        icon: "fas fa-home",
                        requiresAuthentication: true
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Users,
                        L("Users"),
                        url: "Users",
                        icon: "fas fa-users",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Customers,
                        L("Customers"),
                        url: "Customers",
                        icon: "fas fa-people-arrows",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Customers)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Materials,
                        L("Materials"),
                        url: "Materials",
                        icon: "fas fa-snowflake",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Materials)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "CRM",
                        L("CRM"),
                        icon: "fas fa-business-time"
                    ).AddItem(
                            new MenuItemDefinition(
                                PageNames.Estimates,
                                L("Estimates"),
                                url: "CRM/EstimatesList",
                                order: 1,
                                icon: "fas fa-calculator",
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_CRM))
                            ).AddItem(
                            new MenuItemDefinition(
                                PageNames.Enquiries,
                                L("Enquiries"),
                                url: "CRM/EnquiryList",
                                icon: "fas fa-calculator",
                                order: 2,
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_CRM))
                            )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Production,
                        L("Production"),
                        url: "Production",
                        icon: "fas fa-snowflake",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Production)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Planning",
                        L("Planning"),
                        icon: "fas fa-industry"
                    ).AddItem(
                            new MenuItemDefinition(
                                PageNames.WorkOrders,
                                L("Workorders"),
                                url: "Production/WorkorderList",
                                icon: "fas fa-compass",
                                order: 1,
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Workorder))
                            )
                            .AddItem(
                            new MenuItemDefinition(
                                PageNames.Design,
                                L("Design"),
                                url: "Production/Design",
                                icon: "fas fa-palette",
                                order: 2,
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Design))
                            )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Inventory",
                        L("Inventory"),
                        icon: "fas fa-warehouse"
                    ).AddItem(
                            new MenuItemDefinition(
                                PageNames.IssuedInventoryList,
                                L("Inventory"),
                                url: "Inventory",
                                icon: "fas fa-industry",
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Inventory))
                            ).AddItem(
                            new MenuItemDefinition(
                                PageNames.Stores,
                                L("Stores"),
                                url: "Stores",
                                icon: "fas fa-house-user",
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Stores))
                ))
                .AddItem(
                    new MenuItemDefinition(
                        "Purchase",
                        L("Purchase"),
                        icon: "fas fa-file-signature"
                    ).AddItem(
                            new MenuItemDefinition(
                                PageNames.PurchaseOrders,
                                L("PurchaseOrders"),
                                url: "PurchaseOrders",
                                icon: "fas fa-list",
                                order: 2,
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_PurchaseOrder))
                            )
                            .AddItem(
                            new MenuItemDefinition(
                                PageNames.PurchaseReceives,
                                L("PurchaseReceives"),
                                url: "PurchaseReceives",
                                icon: "fas fa-list",
                                order: 4,
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_PurchaseReceive))
                            )
                            .AddItem(
                            new MenuItemDefinition(
                                PageNames.PurchaseIndentList,
                                L("PurchaseIndents"),
                                url: "PurchaseIndents",
                                icon: "fas fa-list",
                                order: 5,
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_PurchaseIndent))
                            )
                            .AddItem(
                            new MenuItemDefinition(
                                PageNames.Vendors,
                                L("Vendors"),
                                url: "Vendors",
                                order: 6,
                                icon: "fas fa-store",
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Vendors))
                            )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Company",
                        L("Company"),
                        icon: "fas fa-building"
                    )
                     .AddItem(
                    new MenuItemDefinition(
                        PageNames.Department,
                        L("Department"),
                        url: "Department",
                        icon: "fas fa-list"
                        , permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Department)
                        )
                    ).AddItem(
                            new MenuItemDefinition(
                                PageNames.Preferences,
                                L("Preferences"),
                                url: "Preferences",
                                icon: "fas fa-list",
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Preferences))
                            )
                    .AddItem(
                            new MenuItemDefinition(
                                PageNames.EditTenant,
                                L("EditTenant"),
                                url: "Tenants/Edit",
                                icon: "fas fa-building",
                                permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_EditCompany))
                            )
                    //.AddItem(
                    //new MenuItemDefinition(
                    //    PageNames.Tenants,
                    //    L("Tenants"),
                    //    url: "Tenants",
                    //    icon: "fas fa-building",
                    //    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants))
                    //)
                //.AddItem(
                //new MenuItemDefinition(
                //    PageNames.EditHostTenant,
                //    L("EditHostInfo"),
                //    url: "Tenants/EditHostInfo",
                //    icon: "fas fa-building",
                //    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants))
                //)

                ).AddItem(
                     new MenuItemDefinition(
                        PageNames.Tenants,
                        L("Tenants"),
                        url: "Tenants",
                        icon: "fas fa-building",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants))
              )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Roles,
                        L("Roles"),
                        url: "Roles",
                        icon: "fas fa-theater-masks",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
              )
           );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ERPackConsts.LocalizationSourceName);
        }
    }
}