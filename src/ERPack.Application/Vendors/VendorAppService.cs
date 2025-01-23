using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AutoMapper.Internal.Mappers;
using ERPack.Stores.Dto;
using ERPack.Stores;
using ERPack.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.Extensions;
using ERPack.Vendors.Dto;
using Abp.Logging;
using Abp.UI;
using Abp.Domain.Entities;

namespace ERPack.Vendors
{
    public class VendorAppService : ERPackAppServiceBase, IVendorAppService
    {
        private readonly IRepository<Vendor, int> _vendorRepository;
        private readonly VendorManager _vendorManager;

        public VendorAppService(IRepository<Vendor> repository,
            VendorManager vendorManager)
        {
            _vendorRepository = repository;
            _vendorManager = vendorManager;
        }

        public async Task<int> CreateAsync(VendorDto input)
        {
            try
            {
                var vendor = ObjectMapper.Map<Vendor>(input);

                int storeId = await _vendorManager.CreateAsync(vendor);

                return storeId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error While Adding Vendor",ex);
                return 0;
            }
        }

        public async Task<VendorDto> UpdateAsync(VendorDto input)
        {
            var entity = await _vendorRepository.GetAsync(input.Id);

            MapToEntity(input, entity);

            var result = await _vendorManager.UpdateAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            var vendor = ObjectMapper.Map<VendorDto>(result);

            return vendor;
        }

        public async Task DeleteAsync(EntityDto<int> input)
        {
            try
            {
                var vendor = await _vendorManager.GetAsync(input.Id);
                _vendorManager.Cancel(vendor);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }

        public Task<PagedResultDto<VendorDto>> GetAllAsync(PagedVendorResultRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            query = ApplySorting(query, input);

            List<Vendor> vendors = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var result = new PagedResultDto<VendorDto>(query.Count(), ObjectMapper.Map<List<VendorDto>>(vendors));
            return Task.FromResult(result);
        }

        public async Task<List<VendorDto>> GetAllVendorsAsync()
        {
            var vendors = await  _vendorManager.GetAllAsync();
            var vendorsList = ObjectMapper.Map<List<VendorDto>>(vendors);
            return vendorsList;
        }

        public async Task<VendorDto> GetAsync(int vendorId)
        {
            var entity = await _vendorManager.GetAsync(vendorId);
            var vendor = ObjectMapper.Map<VendorDto>(entity);
            return vendor;
        }

        protected IQueryable<Vendor> CreateFilteredQuery(PagedVendorResultRequestDto input)
        {
            return _vendorRepository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.VendorName.Contains(input.Keyword)).AsQueryable();
        }

        protected IQueryable<Vendor> ApplySorting(IQueryable<Vendor> query, PagedVendorResultRequestDto input)
        {
            return query.OrderBy(r => r.VendorName);
        }

        protected void MapToEntity(VendorDto input, Vendor vendor)
        {
            ObjectMapper.Map(input, vendor);
        }
    }
}
