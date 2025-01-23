using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ERPack.MultiTenancy.Dto;
using ERPack.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPack.HostTaxation;
using Abp.Collections.Extensions;
using ERPack.Customers.Dto;
using Abp.Logging;
using ERPack.Customers;

namespace ERPack.HostTaxationInfoDetails
{
    public class HostTaxationInfoAppService : ERPackAppServiceBase, IHostTaxationInfoAppService
    {
        private readonly IRepository<HostTaxationInfo, int> _hostTaxationInfoRepository;
        private readonly HostTaxationManager _hostTaxationManager;

        public HostTaxationInfoAppService(IRepository<HostTaxationInfo, int> hostTaxationInfoRepository, HostTaxationManager hostTaxationManager)
        {
            _hostTaxationInfoRepository = hostTaxationInfoRepository;
            _hostTaxationManager = hostTaxationManager;
        }
        public Task<PagedResultDto<HostTaxationDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var hostTaxationInfoList = new List<HostTaxationInfo>();

            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            hostTaxationInfoList = query
                .Skip(input.SkipCount)
                //.Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<HostTaxationDto>(query.Count(), ObjectMapper.Map<List<HostTaxationDto>>(hostTaxationInfoList));
            return Task.FromResult(result);
        }

        public async Task<HostTaxationDto> GetAsync(int hostTaxationId)
        {
            var entity = await _hostTaxationManager.GetAsync(hostTaxationId);
            var hostTaxationDetail = ObjectMapper.Map<HostTaxationDto>(entity);
            return hostTaxationDetail;
        }

        public async Task<(int, string)> CreateAsync(HostTaxationDto input)
        {
            try
            {
                HostTaxationInfo hostTaxationInfo = new HostTaxationInfo();
                hostTaxationInfo = ObjectMapper.Map<HostTaxationInfo>(input);

                if (input.IsDefault == true)
                {
                    int tenantId = (int)input.TenantId;
                    if (tenantId > 0)
                    {
                        var hostTaxationInfoList = new List<HostTaxationInfo>();

                        hostTaxationInfoList = _hostTaxationInfoRepository.GetAll()
                        .Where(x => x.IsDeleted == false && x.TenantId == tenantId).ToList();

                        hostTaxationInfoList.ForEach(x => x.IsDefault = false);

                        foreach (var hostInfo in hostTaxationInfoList)
                        {
                            hostInfo.IsDefault = hostInfo.Id == input.Id;
                            await _hostTaxationManager.UpdateAsync(hostInfo);
                        }
                    }
                }

                int taxationId = await _hostTaxationManager.CreateAsync(hostTaxationInfo);
                return (taxationId, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return (0, ex.Message);
            }
        }

        public async Task<(HostTaxationDto, string)> UpdateAsync(HostTaxationDto input)
        {
            var hostTaxationDetail = new HostTaxationDto();
            try
            {
                var entity = await _hostTaxationInfoRepository.GetAsync(input.Id);
                MapToEntity(input, entity);

                if (input.IsDefault == true)
                {
                    int tenantId = (int)input.TenantId;
                    if (tenantId > 0)
                    {
                        var hostTaxationInfoList = new List<HostTaxationInfo>();

                        hostTaxationInfoList = _hostTaxationInfoRepository.GetAll()
                        .Where(x => x.IsDeleted == false && x.TenantId == tenantId).ToList();

                        hostTaxationInfoList.ForEach(x => x.IsDefault = false);

                        foreach (var hostInfo in hostTaxationInfoList)
                        {
                            hostInfo.IsDefault = hostInfo.Id == input.Id;
                            await _hostTaxationManager.UpdateAsync(hostInfo);
                        }
                    }
                }

                var result = await _hostTaxationManager.UpdateAsync(entity);

                await CurrentUnitOfWork.SaveChangesAsync();

                hostTaxationDetail = ObjectMapper.Map<HostTaxationDto>(result);

                return (hostTaxationDetail, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return (hostTaxationDetail, ex.Message);
            }
        }

        public async Task ChangeDefaultDetailsAsync(EntityDto<int> input)
        {
            int tenantId = AbpSession.TenantId.Value;
            if (tenantId > 0)
            {
                var hostTaxationInfoList = new List<HostTaxationInfo>();

                hostTaxationInfoList = _hostTaxationInfoRepository.GetAll()
                .Where(x => x.IsDeleted == false && x.TenantId == tenantId).ToList();

                hostTaxationInfoList.ForEach(x => x.IsDefault = false);

                var chageIsDefault = hostTaxationInfoList.Where(x => x.Id == input.Id).FirstOrDefault();
                if (chageIsDefault != null)
                {
                    chageIsDefault.IsDefault = true;
                }

                foreach (var hostInfo in hostTaxationInfoList)
                {
                    await _hostTaxationManager.UpdateAsync(hostInfo);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task<List<HostTaxationDto>> GetTaxationDetailsByTenantId(int tenentId)
        {
            List<HostTaxationDto> hostTaxationInfoList = new List<HostTaxationDto>();

            var hostTaxationList = (await _hostTaxationManager.GetAllAsync()).Where(x => x.TenantId == tenentId);
            if (hostTaxationList != null && hostTaxationList.Count() > 0)
            {
                foreach(var hostInfo in hostTaxationList)
                {
                    hostTaxationInfoList.Add(ObjectMapper.Map<HostTaxationDto>(hostInfo));
                }
            }
            return hostTaxationInfoList;
        }

        protected IQueryable<HostTaxationInfo> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return _hostTaxationInfoRepository.GetAll()
                .Where(x => x.IsDeleted == false).AsQueryable();
        }

        protected IQueryable<HostTaxationInfo> ApplySorting(IQueryable<HostTaxationInfo> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.AddressName);
        }

        protected void MapToEntity(HostTaxationDto input, HostTaxationInfo hostTaxationInfo)
        {
            ObjectMapper.Map(input, hostTaxationInfo);
        }
    }
}
