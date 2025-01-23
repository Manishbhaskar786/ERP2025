using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ERPack.Authorization;
using ERPack.Departments;
using ERPack.Departments.Dto;
using ERPack.Designs;
using ERPack.Designs.Dto;
using ERPack.Enquiries;
using ERPack.Enquiries.Dto;
using ERPack.Enquries.Dto;
using ERPack.Estimates;
using ERPack.Estimates.Dto;
using ERPack.Inventory;
using ERPack.Inventory.Dto;
using ERPack.Materials;
using ERPack.Materials.Dto;
using ERPack.PurchaseIndents;
using ERPack.PurchaseIndents.Dto;
using ERPack.PurchaseOrders;
using ERPack.PurchaseOrders.Dto;
using ERPack.PurchaseReceives.Dto;
using ERPack.PurchaseRecieves;
using ERPack.Workorders;
using ERPack.Workorders.Dto;
using ERPack.WorkOrders;

namespace ERPack
{
    [DependsOn(
        typeof(ERPackCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ERPackApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ERPackAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
                config.CreateMap<Material, MaterialDto>()
                      .ForMember(u => u.ItemTypeName, options => options.MapFrom(input => input.ItemType.ItemTypeName))
                      .ForMember(u => u.DepartmentName, options => options.MapFrom(input => input.Department.DeptName))
                      .ForMember(u => u.BuyingUnit, options => options.MapFrom(input => input.BuyingUnit.UnitName))
                      .ForMember(u => u.SellingUnit, options => options.MapFrom(input => input.SellingUnit.UnitName));

                config.CreateMap<Enquiry, EnquiryDto>()
                      .ForMember(u => u.CustomerName, options => options.MapFrom(input => input.Customer.Name))
                      .ForMember(u => u.BoardTypeName, options => options.MapFrom(input => input.BoardType.BoardTypeName));

                config.CreateMap<Estimate, EstimateDto>()
                      .ForMember(u => u.CustomerName, options => options.MapFrom(input => input.Customer.Name));

                config.CreateMap<EstimateTask, EstimateTaskDto>()
                      .ForMember(u => u.MaterialName, options => options.MapFrom(input => input.Material.DisplayName))
                      .ForMember(u => u.DepartmentId, options => options.MapFrom(input => input.Material.DepartmentId))
                      .ForMember(u => u.DepartmentName, options => options.MapFrom(input => input.Material.Department.DeptName))
                      .ForMember(u => u.ItemCode, options => options.MapFrom(input => input.Material.ItemCode))
                      .ForMember(u => u.SellingUnitName, options => options.MapFrom(input => input.Unit.UnitName));

                config.CreateMap<WorkorderTask, WorkorderTaskDto>()
                     .ForMember(u => u.MaterialName, options => options.MapFrom(input => input.Material.DisplayName))
                     .ForMember(u => u.ItemCode, options => options.MapFrom(input => input.Material.ItemCode))
                     .ForMember(u => u.UserName, options => options.MapFrom(input => input.User.FullName))
                     .ForMember(u => u.WorkorderSubTaskCode, options => options.MapFrom(input => input.WorkorderSubTask.WorkorderSubTaskId))
                     .ForMember(u => u.DepartmentName, options => options.MapFrom(input => input.Material.Department.DeptName))
                     .ForMember(u => u.WorkorderCode, options => options.MapFrom(input => input.Workorder.WorkorderId));

                config.CreateMap<BoardType, DesignDto>()
                      .ForMember(u => u.BoardTypeName, options => options.MapFrom(input => input.BoardTypeName));

                config.CreateMap<Workorder, WorkorderDto>()
                      .ForPath(u => u.EnquiryCode, options => options.MapFrom(input => input.Estimate.Enquiry.EnquiryId))
                      .ForPath(u => u.EstimateCode, options => options.MapFrom(input => input.Estimate.EstimateId)).ReverseMap();

                config.CreateMap<Design, DesignDto>()
                      .ForMember(u => u.BoardTypeId, options => options.MapFrom(input => input.Enquiry.BoardTypeId))
                      .ForMember(u => u.DesignUserId, options => options.MapFrom(input => input.Enquiry.DesignUserId))
                      .ForMember(u => u.DesignName, options => options.MapFrom(input => input.Enquiry.DesignName))
                      .ForMember(u => u.DesignImage, options => options.MapFrom(input => input.Enquiry.DesignImage))
                      .ForMember(u => u.DesignNumber, options => options.MapFrom(input => input.Enquiry.DesignNumber))
                      .ForMember(u => u.CustomerId, options => options.MapFrom(input => input.Enquiry.CustomerId))
                      .ForMember(u => u.BoxLength, options => options.MapFrom(input => input.Enquiry.BoxLength))
                      .ForMember(u => u.BoxWidth, options => options.MapFrom(input => input.Enquiry.BoxWidth))
                      .ForMember(u => u.BoxHeight, options => options.MapFrom(input => input.Enquiry.BoxHeight))
                      .ForMember(u => u.SheetSizeWidth, options => options.MapFrom(input => input.Enquiry.SheetSizeWidth))
                      .ForMember(u => u.SheetSizeLength, options => options.MapFrom(input => input.Enquiry.SheetSizeLength))
                      .ForMember(u => u.EnquiryId, options => options.MapFrom(input => input.Enquiry.Id))
                      .ForMember(u => u.EnquiryNumber, options => options.MapFrom(input => input.Enquiry.EnquiryId));

                config.CreateMap<DesignMaterial, DesignMaterialDto>()
                      .ForMember(u => u.DisplayName, options => options.MapFrom(input => input.Material.DisplayName))
                      .ForMember(u => u.SellingPrice, options => options.MapFrom(input => input.Material.SellingPrice))
                      .ForMember(u => u.SellingUnitId, options => options.MapFrom(input => input.Material.SellingUnitId))
                      .ForMember(u => u.SGST, options => options.MapFrom(input => input.Material.SGST))
                      .ForMember(u => u.ItemCode, options => options.MapFrom(input => input.Material.ItemCode))
                      .ForMember(u => u.IGST, options => options.MapFrom(input => input.Material.IGST))
                      .ForMember(u => u.CGST, options => options.MapFrom(input => input.Material.CGST));

                config.CreateMap<InventoryIssued, InventoryIssuedDto>()
                      .ForMember(u => u.IssuedBy, options => options.MapFrom(input => input.User.FullName));

                config.CreateMap<InventoryIssuedItem, InventoryIssuedItemDto>()
                      .ForMember(u => u.IssuedBy, options => options.MapFrom(input => input.User.FullName))
                      .ForMember(u => u.ItemType, options => options.MapFrom(input => input.ItemType.ItemTypeName))
                      .ForMember(u => u.MaterialName, options => options.MapFrom(input => input.Material.DisplayName))
                      .ForMember(u => u.MaterialCode, options => options.MapFrom(input => input.Material.ItemCode))
                      .ForMember(u => u.DepartmentName, options => options.MapFrom(input => input.Department.DeptName))
                      .ForMember(u => u.FromStoreName, options => options.MapFrom(input => input.FromStore.StoreName))
                      .ForMember(u => u.ToStoreName, options => options.MapFrom(input => input.ToStore.StoreName));

                config.CreateMap<PurchaseOrder, PurchaseOrderDto>()
                      .ForMember(u => u.VendorName, options => options.MapFrom(input => input.Vendor.VendorName));

                config.CreateMap<PurchaseOrderItem, PurchaseOrderItemDto>()
                      .ForMember(u => u.ItemType, options => options.MapFrom(input => input.ItemType.ItemTypeName))
                      .ForMember(u => u.MaterialName, options => options.MapFrom(input => input.Material.DisplayName))
                      .ForMember(u => u.MaterialCode, options => options.MapFrom(input => input.Material.ItemCode))
                      .ForMember(u => u.VendorId, options => options.MapFrom(input => input.PurchaseOrder.VendorId))
                      .ForMember(u => u.VendorName, options => options.MapFrom(input => input.PurchaseOrder.Vendor.VendorName))
                      .ForMember(u => u.BuyingUnit, options => options.MapFrom(input => input.Unit.UnitName));

                config.CreateMap<PurchaseReceive, PurchaseReceiveDto>()
                     .ForMember(u => u.VendorName, options => options.MapFrom(input => input.Vendor.VendorName))
                     .ForMember(u => u.POCode, options => options.MapFrom(input => input.PurchaseOrder.POCode));

                config.CreateMap<PurchaseReceiveItem, PurchaseReceiveItemDto>()
                     .ForMember(u => u.MaterialName, options => options.MapFrom(input => input.PurchaseOrderItem.Material.DisplayName))
                     .ForMember(u => u.Unit, options => options.MapFrom(input => input.PurchaseOrderItem.Unit.UnitName))
                     .ForMember(u => u.QuantityOrdered, options => options.MapFrom(input => input.PurchaseOrderItem.Quantity))
                     .ForMember(u => u.StoreName, options => options.MapFrom(input => input.Store.StoreName));

                config.CreateMap<PurchaseIndent, PurchaseIndentDto>()
                     .ForMember(u => u.ItemCode, options => options.MapFrom(input => input.Material.ItemCode))
                     .ForMember(u => u.RequestedByUser, options => options.MapFrom(input => input.User.FullName))
                     .ForMember(u => u.MaterialName, options => options.MapFrom(input => input.Material.DisplayName));

                config.CreateMap<InventoryRequest, InventoryRequestOutput>()
                     .ForMember(u => u.RequestedBy, options => options.MapFrom(input => input.User.FullName));

                config.CreateMap<EnquiryMaterial, EnquiryMaterialDto>()
                      .ForMember(u => u.ItemCode, options => options.MapFrom(input => input.Material.ItemCode))
                      .ForMember(u => u.MaterialName, options => options.MapFrom(input => input.Material.DisplayName));

                config.CreateMap<Department, DepartmentDto>()
                     .ForMember(u => u.DeptName, options => options.MapFrom(input => input.DeptName))
                     .ForMember(u => u.Description, options => options.MapFrom(input => input.Description));

                config.CreateMap<DepartmentOutput, Department>()
                    .ForMember(u => u.DeptName, options => options.MapFrom(input => input.DeptName))
                    .ForMember(u => u.Description, options => options.MapFrom(input => input.Description));
            });
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ERPackApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
