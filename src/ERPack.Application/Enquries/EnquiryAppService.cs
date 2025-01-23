using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Logging;
using Abp.UI;
using ERPack.Authorization;
using ERPack.Common.Dto;
using ERPack.Enquiries.Dto;
using ERPack.Enquries.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Enquiries
{
    [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Enquiry)]
    public class EnquiryAppService : ERPackAppServiceBase, IEnquiryAppService
    {
        readonly IRepository<Enquiry, long> _enquiryRepository;
        private readonly EnquiryManager _enquiryManager;
        private readonly EnquiryMaterialManager _enquiryMaterialManager;

        public EnquiryAppService(IRepository<Enquiry, long> enquiryRepository,
            EnquiryManager enquiryManager,
            EnquiryMaterialManager enquiryMaterialManager)
        {
            _enquiryRepository = enquiryRepository;
            _enquiryManager = enquiryManager;
            _enquiryMaterialManager = enquiryMaterialManager;
        }

        public async Task<long> CreateAsync(EnquiryDto input)
        {
            try
            {
                var enquiry = ObjectMapper.Map<Enquiry>(input);

                long enquiryId = await _enquiryManager.CreateAsync(enquiry);

                return enquiryId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }

        }

        public async Task<EnquiryDto> UpdateAsync(EnquiryDto input)
        {
            try
            {
                var entity = await _enquiryRepository.GetAsync(input.Id);

                input.CreationTime = entity.CreationTime;
                input.CreatorUserId = entity.CreatorUserId;
                input.Status = entity.Status;
                input.StatusDatetime = entity.StatusDatetime;
                input.IsEstimateApproved = entity.IsEstimateApproved;
                input.DesignImage ??= entity.DesignImage;
                MapToEntity(input, entity);


                var result = await _enquiryManager.UpdateAsync(entity);

                if (input.EnquiryMaterials?.Count == 0)
                {
                    await _enquiryMaterialManager.DeleteEnquiryMaterialByEnquiryIdsAsync(input.Id);
                }

                await CurrentUnitOfWork.SaveChangesAsync();

                var enquiry = ObjectMapper.Map<EnquiryDto>(result);

                return enquiry;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return null;
            }
        }

        public async Task<EnquiryDto> UpdateStatusAsync(long enquiryId, string status)
        {
            try
            {
                var entity = await _enquiryRepository.GetAsync(enquiryId);

                entity.Status = status;
                entity.StatusDatetime = DateTime.Now;

                var result = await _enquiryManager.UpdateAsync(entity);

                await CurrentUnitOfWork.SaveChangesAsync();

                var enquiry = ObjectMapper.Map<EnquiryDto>(result);

                return enquiry;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return null;
            }
        }
        
        public async Task<EnquiryDto> GetAsync(long enquiryId)
        {
            var entity = await _enquiryManager.GetAsync(enquiryId);
            var enquiry = ObjectMapper.Map<EnquiryDto>(entity);
            return enquiry;
        }

        public Task<PagedResultDto<EnquiryDto>> GetAllAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<Enquiry> enquires = query
                .Skip(input.SkipCount)
                //.Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<EnquiryDto>(query.Count(), ObjectMapper.Map<List<EnquiryDto>>(enquires));
            return Task.FromResult(result);
        }

        public Task<PagedResultDto<EnquiryDto>> GetAllDesignReadyAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateFilteredQueryForDesignReady(input);

            query = query.Where(x => x.Status == ERPackConsts.Design);

            query = ApplySorting(query, input);

            List<Enquiry> enquires = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<EnquiryDto>(query.Count(), ObjectMapper.Map<List<EnquiryDto>>(enquires));
            return Task.FromResult(result);
        }

        public async Task<List<EnquiryDto>> GetAllEnquriesAsync()
        {
            var enquiries = await _enquiryManager.GetAllAsync();

            var result = ObjectMapper.Map<List<EnquiryDto>>(enquiries);

            return result;
        }

        public async Task DeleteAsync(EntityDto<long> input)
        {
            try
            {
                var enquiry = await _enquiryManager.GetAsync(input.Id);
                if (enquiry == null) return;

                await _enquiryMaterialManager.DeleteEnquiryMaterialByEnquiryIdsAsync(input.Id);
                await _enquiryManager.DeleteAsync(enquiry);

                await CurrentUnitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        protected IQueryable<Enquiry> CreateFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _enquiryRepository.GetAllIncluding(x => x.Customer)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.EnquiryId.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Enquiry> CreateFilteredQueryForDesignReady(CommonPagedResultRequestDto input)
        {
            return _enquiryRepository.GetAllIncluding(x => x.Customer).Where(x => x.Status == ERPackConsts.Design)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.EnquiryId.Contains(input.Keyword)).AsQueryable();
        }


        protected IQueryable<Enquiry> ApplySorting(IQueryable<Enquiry> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }

        protected void MapToEntity(EnquiryDto input, Enquiry enquiry)
        {
            ObjectMapper.Map(input, enquiry);
        }

        public async Task<List<EnquiryMaterialDto>> GetEnquiryMaterialsAsync(int enquiryId)
        {
            var enquiryMaterials = await _enquiryMaterialManager.GetAllByEnquiryIdAsync(enquiryId);

            var result = ObjectMapper.Map<List<EnquiryMaterialDto>>(enquiryMaterials);

            return result;
        }

        public async Task DeleteDesignMaterialAsync(int enquiryMaterialId)
        {
            await _enquiryMaterialManager.DeleteEnquiryMaterialsAsync(enquiryMaterialId);

        }
    }
}
