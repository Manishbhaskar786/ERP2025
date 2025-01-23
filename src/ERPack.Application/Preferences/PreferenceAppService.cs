using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Logging;
using ERPack.Authorization;
using ERPack.Authorization.Users;
using ERPack.Customers;
using ERPack.Customers.Dto;
using ERPack.Designs;
using ERPack.Enquiries;
using ERPack.Estimates;
using ERPack.Inventory;
using ERPack.Materials;
using ERPack.Preferences.Dto;
using ERPack.PurchaseOrders;
using ERPack.Stores;
using ERPack.Stores.Dto;
using ERPack.Vendors.Dto;
using ERPack.Workorders;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Preferences
{
    /// <summary>
    /// Service to Add Preferences for users like Id's formatting.
    /// </summary>
    public class PreferenceAppService : ERPackAppServiceBase, IPreferenceAppService
    {
        readonly IRepository<Preference, int> _preferenceRepository;
        private readonly PreferenceManager _preferenceManager;
        private readonly CustomerManager _customerManager;
        private readonly VendorManager _vendorManager;
        private readonly MaterialManager _materialManager;
        private readonly EnquiryManager _enquiryManager;
        private readonly DesignManager _designManager;
        private readonly EstimateManager _estimateManager;
        private readonly WorkorderManager _workorderManager;
        private readonly InventoryManager _inventoryManager;
        private readonly PurchaseOrderManager _purchaseOrderManager;
        private readonly InventoryRequestManager _inventoryRequestManager;
        /// <summary>
        /// Constructor for PreferenceAppService.
        /// </summary>
        public PreferenceAppService(IRepository<Preference> repository,
            PreferenceManager preferenceManager,
            CustomerManager customerManager,
            VendorManager vendorManager,
            MaterialManager materialManager,
            EnquiryManager enquiryManager,
            DesignManager designManager,
            EstimateManager estimateManager,
            WorkorderManager workorderManager,
            InventoryManager inventoryManager,
            PurchaseOrderManager purchaseOrderManager,
            InventoryRequestManager inventoryRequestManager)
        {
            _preferenceRepository = repository;
            _preferenceManager = preferenceManager;
            _customerManager = customerManager;
            _vendorManager = vendorManager;
            _materialManager = materialManager;
            _enquiryManager = enquiryManager;
            _designManager = designManager;
            _estimateManager = estimateManager;
            _workorderManager = workorderManager;
            _inventoryManager = inventoryManager;
            _purchaseOrderManager = purchaseOrderManager;
            _inventoryRequestManager = inventoryRequestManager;
        }

        /// <summary>
        /// This method Create New Preference.
        /// </summary>
        /// <param name="input">input for adding preference of PreferenceDto type.</param>
        /// <returns>Newly created id in prefernce table</returns>
        [AbpAuthorize(PermissionNames.Pages_Preferences)]
        public async Task<int> CreateAsync(PreferenceDto input)
        {
            try
            {
                var entity = await _preferenceManager.GetByIdTypeAsync(input.IdType);

                if (entity != null)
                {
                    return 0;
                }

                var preference = ObjectMapper.Map<Preference>(input);

                int preferenceId = await _preferenceManager.CreateAsync(preference);

                return preferenceId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error in Creating Perference", ex);
                return -1;
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Preferences)]
        public async Task<PreferenceDto> UpdateAsync(PreferenceDto input)
        {
            var entity = await _preferenceRepository.GetAsync(input.Id);

            MapToEntity(input, entity);

            var result = await _preferenceManager.UpdateAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            var preference = ObjectMapper.Map<PreferenceDto>(result);

            return preference;
        }

        [AbpAuthorize(PermissionNames.Pages_Preferences)]
        public Task<PagedResultDto<PreferenceDto>> GetAllAsync(PagedStoreResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<Preference> preferences = query
                .Skip(input.SkipCount)
               // .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<PreferenceDto>(query.Count(), ObjectMapper.Map<List<PreferenceDto>>(preferences));
            return Task.FromResult(result);
        }

        [AbpAuthorize(PermissionNames.Pages_Preferences)]
        public async Task<PreferenceDto> GetAsync(int preferenceId)
        {
            var entity = await _preferenceManager.GetAsync(preferenceId);
            var store = ObjectMapper.Map<PreferenceDto>(entity);
            return store;
        }

        [AbpAuthorize(PermissionNames.Pages_Preferences)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var entity = await _preferenceManager.GetAsync(input.Id);
            _preferenceManager.Cancel(entity);
        }

        public async Task<string> GetByNameAsync(string idType, string name = "")
        {
            string Id = string.Empty;
            var entity = await _preferenceManager.GetByIdTypeAsync(idType);
            if (entity != null)
            {
                if (entity.NameIdentifier.Equals("1stTwoCharacters") && !string.IsNullOrWhiteSpace(name))
                {
                    string[] words = name.Split(' ');

                    // Get the first two characters of each word
                    string result = "";
                    foreach (string word in words)
                    {
                        if (word.Length >= 1)
                        {
                            result += word.Substring(0, 1);
                        }
                    }

                    Id += result;
                }
                else if (entity.NameIdentifier.Equals("2Charactersof1stWord") && !string.IsNullOrWhiteSpace(name))
                {
                    Id += name.Substring(0, 2);
                }

                if (!string.IsNullOrWhiteSpace(entity.FixedName))
                {
                    Id += entity.FixedName.Trim();
                }
                if (entity.MonthSelection == "Yes")
                {
                    Id += DateTime.Now.ToString("MMM").ToUpper();
                }
                if (entity.YearSelection == "Yes")
                {
                    Id += DateTime.Now.Year.ToString();
                }
            }

            if (idType.Equals("CustomerId"))
            {
                var customersCount = _customerManager.GetAllAsync().Result.Count;
                Id += (customersCount + 1).ToString("D4");
            }
            else if (idType.Equals("VendorId"))
            {
                var vendorsCount = _vendorManager.GetAllAsync().Result.Count;
                Id += (vendorsCount + 1).ToString("D4");
            }
            else if (idType.Equals("MaterialId"))
            {
                var materialsCount = _materialManager.GetAllAsync().Result.Count;
                Id += (materialsCount + 1).ToString("D4");
            }
            else if (idType.Equals("EnquiryId"))
            {
                /*var enquiries = await _enquiryManager.GetAllAsync();
                var maxEnquiryNumber = enquiries
                    .Where(e => e.EnquiryId.StartsWith(Id))
                    .Select(e => int.Parse(e.EnquiryId.Replace(Id, "")))
                    .DefaultIfEmpty(0)
                    .Max();

                Id += (maxEnquiryNumber + 1).ToString("D4");*/
                var enquiries = _enquiryManager.GetAllAsync().Result.Count;
                Id += (enquiries + 1).ToString("D4");
            }
            else if (idType.Equals("DesignId"))
            {
                //var designs = await _designManager.GetAllAsync();
                //var maxDesignNumber = designs
                //    .Where(e => e.DesignId != null && e.DesignId.StartsWith(Id))
                //    .Select(e => int.Parse(e.DesignId.Replace(Id, "")))
                //    .DefaultIfEmpty(0)
                //    .Max();

                //Id += (maxDesignNumber + 1).ToString("D4");

                var maxDesignNumber = _designManager.GetAllAsync().Result.Count;
                Id += (maxDesignNumber + 1).ToString("D4");
            }
            else if (idType.Equals("EstimateId"))
            {
                var estimatesCount = _estimateManager.GetAllAsync().Result.Count;
                Id += (estimatesCount + 1).ToString("D4");
            }
            else if (idType.Equals("WorkorderId"))
            {
                var workordersCount = _workorderManager.GetAllAsync().Result.Count;
                Id += (workordersCount + 1).ToString("D4");
            }
            else if (idType.Equals("WorkorderTaskId"))
            {
                var workorderSubTasksCount = _workorderManager.GetAllWorkorderSubTasksAsync().Result.Count;
                Id += (workorderSubTasksCount + 1).ToString("D4");
            }
            else if (idType.Equals("InventoryIssueId"))
            {
                var inventoryCount = _inventoryManager.GetAllAsync().Result.Count;
                Id += (inventoryCount + 1).ToString("D4");
            }
            else if (idType.Equals("InventoryReqId"))
            {
                var inventoryReqCount = _inventoryRequestManager.GetAllAsync().Result.Count;
                Id += (inventoryReqCount + 1).ToString("D4");
            }
            else if (idType.Equals("PurchaseOrderId"))
            {
                var purchaseOrderCount = _purchaseOrderManager.GetAllAsync().Result.Count;
                Id += (purchaseOrderCount + 1).ToString("D4");
            }
            return Id.ToUpper();
        }

        protected IQueryable<Preference> CreateFilteredQuery(PagedStoreResultRequestDto input)
        {
            return _preferenceRepository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.FixedName.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Preference> ApplySorting(IQueryable<Preference> query, PagedStoreResultRequestDto input)
        {
            return query.OrderBy(r => r.FixedName);
        }

        protected void MapToEntity(PreferenceDto input, Preference preference)
        {
            ObjectMapper.Map(input, preference);
        }

    }
}
