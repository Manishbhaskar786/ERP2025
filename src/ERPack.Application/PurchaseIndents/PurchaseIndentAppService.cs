using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using ERPack.Authorization;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using ERPack.PurchaseOrders.Dto;
using ERPack.Common.Dto;
using Abp.Collections.Extensions;
using ERPack.PurchaseOrders;
using ERPack.PurchaseIndents.Dto;
using Abp.Logging;
using Microsoft.EntityFrameworkCore;
using ERPack.PurchaseRecieves;

namespace ERPack.PurchaseIndents
{
    [AbpAuthorize(PermissionNames.Pages_PurchaseIndent)]
    public class PurchaseIndentAppService : ERPackAppServiceBase, IPurchaseIndentAppService
    {
        readonly IRepository<PurchaseIndent, long> _purchaseIndentRepository;
        private readonly PurchaseIndentManager _purchaseIndentManager;

        public PurchaseIndentAppService(IRepository<PurchaseIndent, long> purchaseIndentRepository,
            PurchaseIndentManager purchaseIndentManager)
        {
            _purchaseIndentRepository = purchaseIndentRepository;
            _purchaseIndentManager = purchaseIndentManager;
        }

        public async Task<long> CreateAsync(PurchaseIndentDto input)
        {
            try
            {
                var purchaseIndent = ObjectMapper.Map<PurchaseIndent>(input);

                long purchaseIndentId = await _purchaseIndentManager.CreateAsync(purchaseIndent);

                return purchaseIndentId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public Task<PagedResultDto<PurchaseIndentDto>> GetAllAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<PurchaseIndent> purchaseIndents = query
                .Skip(input.SkipCount)
              //  .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<PurchaseIndentDto>(query.Count(), ObjectMapper.Map<List<PurchaseIndentDto>>(purchaseIndents));
            return Task.FromResult(result);
        }


        protected IQueryable<PurchaseIndent> CreateFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _purchaseIndentRepository.GetAll().Include(x=> x.Material).Include(x=> x.User)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Material.ItemCode.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<PurchaseIndent> ApplySorting(IQueryable<PurchaseIndent> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }

    }
}
