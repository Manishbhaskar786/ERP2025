using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ERPack.Authorization
{
    public class ERPackAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Customers, L("Customers"));
            context.CreatePermission(PermissionNames.Pages_Materials, L("Materials"));
            context.CreatePermission(PermissionNames.Pages_Stores, L("Stores"));
            context.CreatePermission(PermissionNames.Pages_Vendors, L("Vendors"));
            context.CreatePermission(PermissionNames.Pages_Preferences, L("Preferences"));
            context.CreatePermission(PermissionNames.Pages_Inventory, L("Inventory"));
            context.CreatePermission(PermissionNames.Pages_PurchaseOrder, L("PurchaseOrder"));
            context.CreatePermission(PermissionNames.Pages_PurchaseReceive, L("PurchaseReceive"));
            context.CreatePermission(PermissionNames.Pages_PurchaseIndent, L("PurchaseIndent"));
            context.CreatePermission(PermissionNames.Pages_CRM, L("CRM"));
            context.CreatePermission(PermissionNames.Pages_Production, L("Production"));
            context.CreatePermission(PermissionNames.Pages_Enquiry, L("Enquiry"));
            context.CreatePermission(PermissionNames.Pages_Workorder, L("Workorder"));
            context.CreatePermission(PermissionNames.Pages_Design, L("Design"));
            context.CreatePermission(PermissionNames.Pages_EditCompany, L("EditComapny"), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.Pages_Department, L("Department"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ERPackConsts.LocalizationSourceName);
        }
    }
}
