using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using ERPack.Common.Dto;
using ERPack.Enquiries.Dto;
using ERPack.Enquiries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Extensions;
using System.Linq;
using ERPack.Designs.Dto;
using Abp.Domain.Repositories;
using Abp.Collections.Extensions;
using Abp.Logging;
using ERPack.Designs.DesignMaterials;
using Microsoft.EntityFrameworkCore;

namespace ERPack.Designs
{
    [AbpAuthorize]
    public class DesignAppService : ERPackAppServiceBase, IDesignAppService
    {
        readonly IRepository<Design, long> _designRepository;
        private readonly DesignManager _designManager;
        private readonly DesignMaterialManager _designMaterialManager;

        public DesignAppService(DesignManager designManager,
            DesignMaterialManager designMaterialManager,
            IRepository<Design, long> designRepository)
        {
            _designRepository = designRepository;
            _designManager = designManager;
            _designMaterialManager = designMaterialManager;
        }

        #region Design Methods

        public async Task<long> CreateAsync(DesignDto input)
        {
            try
            {
                var design = ObjectMapper.Map<Design>(input);

                long designId = await _designManager.CreateAsync(design);

                return designId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<DesignDto> UpdateAsync(DesignDto input)
        {
            try
            {
                var entity = await _designRepository.GetAsync(input.Id);

                input.CreationTime = entity.CreationTime;
                input.CreatorUserId = entity.CreatorUserId;
                input.Status = entity.Status;
                input.ReportDoc ??= entity.ReportDoc;
                input.CompletionDatetime = entity.CompletionDatetime;

                MapToEntity(input, entity);

                var result = await _designManager.UpdateAsync(entity);

                if (input.DesignMaterials?.Count == 0)
                {
                    await _designMaterialManager.DeleteDesignMaterialByDesignIdsAsync(input.Id);
                }

                await CurrentUnitOfWork.SaveChangesAsync();

                var design = ObjectMapper.Map<DesignDto>(result);

                return design;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return null;
            }
        }

        public async Task<DesignDto> UpdateStatusAsync(long designId, string status)
        {
            try
            {
                var entity = await _designRepository.GetAsync(designId);

                entity.Status = status;

                var result = await _designManager.UpdateAsync(entity);

                await CurrentUnitOfWork.SaveChangesAsync();

                var enquiry = ObjectMapper.Map<DesignDto>(result);

                return enquiry;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return null;
            }
        }
        public async Task DeleteAsync(EntityDto<long> input)
        {
            try
            {
                var design = await _designManager.GetAsync(input.Id);
                _designManager.Cancel(design);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }

        }
        public async Task<DesignDto> GetAsync(long designId)
        {
            var entity = await _designManager.GetAsync(designId);
            var design = ObjectMapper.Map<DesignDto>(entity);
            return design;
        }
        public Task<PagedResultDto<DesignDto>> GetAllAsync(CommonPagedResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<Design> designs = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<DesignDto>(query.Count(), ObjectMapper.Map<List<DesignDto>>(designs));
            return Task.FromResult(result);
        }

        public Task<PagedResultDto<DesignDto>> GetCompletedDesignsAsync(CommonPagedResultRequestDto input)
        {
            try
            {
                var query = _designRepository.GetAll()
                    .Include(x => x.Enquiry)
                    .Where(x => x.Status == "Estimate")
                    .OrderByDescending(x => x.CompletionDatetime)
                    .AsQueryable();

                List<Design> completedDesigns = query
                .Skip(input.SkipCount)
                //.Take(input.MaxResultCount)
                .ToList();

                var result = new PagedResultDto<DesignDto>(query.Count(), ObjectMapper.Map<List<DesignDto>>(completedDesigns));

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }

        public async Task<List<DesignDto>> GetAllCompletedDesignsAsync()
        {
            try
            {
                var completedDesigns = await _designRepository.GetAll().Where(x => x.Status == ERPackConsts.Estimate)
                    .OrderByDescending(x => x.CompletionDatetime).ToListAsync();

                return ObjectMapper.Map<List<DesignDto>>(completedDesigns);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }

        #endregion

        #region Design Materials Methods

        public async Task<List<DesignMaterialDto>> GetDesignMaterialsAsync(long designId)
        {
            var entity = await _designMaterialManager.GetDesignMaterialsAsync(designId);
            var designMaterials = ObjectMapper.Map<List<DesignMaterialDto>>(entity);
            return designMaterials;
        }

        public async Task<DesignMaterialDto> GetDesignMaterialAsync(int id)
        {
            var entity = await _designMaterialManager.GetAsync(id);
            var designMaterial = ObjectMapper.Map<DesignMaterialDto>(entity);
            return designMaterial;
        }

        public async Task<long> AddDesignMaterialsAsync(DesignMaterialDto input)
        {
            try
            {
                var designMaterial = ObjectMapper.Map<DesignMaterial>(input);

                long designMaterialId = await _designMaterialManager.CreateAsync(designMaterial);

                return designMaterialId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return 0;
            }
        }

        public async Task<DesignMaterialDto> UpdateDesignMaterialAsync(DesignMaterialDto input)
        {
            try
            {
                var entity = await _designMaterialManager.GetAsync(input.Id.Value);

                input.CreationTime = entity.CreationTime;
                input.CreatorUserId = entity.CreatorUserId;
                input.DesignId = entity.DesignId;

                MapToDesignMaterialEntity(input, entity);

                var result = await _designMaterialManager.UpdateAsync(entity);

                await CurrentUnitOfWork.SaveChangesAsync();

                var designMaterial = ObjectMapper.Map<DesignMaterialDto>(result);

                return designMaterial;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return null;
            }
        }

        public async Task DeleteDesignMaterialAsync(int id)
        {
            try
            {
                var designMaterial = await _designMaterialManager.GetAsync(id);
                _designMaterialManager.Cancel(designMaterial);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }

        }

        #endregion

        #region Design Related Table Methods

        public async Task<List<DesignSheet>> GetDesignNamesAsync(string name)
        {
            try
            {
                var designNames = await _designManager.GetDesignNamesAsync(name);

                return designNames;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }

        public async Task<List<BoardType>> GetBoardTypesAsync(string query)
        {
            try
            {
                var boardTypes = await _designManager.GetBoardTypesAsync(query);

                return boardTypes;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }

        public async Task<List<ToolConfiguration>> GetToolConfigurationsAsync()
        {
            try
            {
                var toolConfigurations = await _designManager.GetToolConfigurationsAsync();

                return toolConfigurations;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }

        #endregion

        #region Private Methods
        protected IQueryable<Design> CreateFilteredQuery(CommonPagedResultRequestDto input)
        {
            return _designRepository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.DesignId.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Design> ApplySorting(IQueryable<Design> query, CommonPagedResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime);
        }

        protected void MapToEntity(DesignDto input, Design design)
        {
            ObjectMapper.Map(input, design);
        }

        protected void MapToDesignMaterialEntity(DesignMaterialDto input, DesignMaterial designMaterial)
        {
            ObjectMapper.Map(input, designMaterial);
        }
        #endregion
    }
}
