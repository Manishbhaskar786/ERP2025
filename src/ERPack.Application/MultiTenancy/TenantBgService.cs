using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using ERPack.Customers;
using ERPack.MultiTenancy.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.MultiTenancy
{
    public class TenantBgService : ITenantBgService
    {
        private readonly IRepository<Tenant, int> _tenantRepository;
        //private readonly TenantManager _tenantManager;

        public TenantBgService(IRepository<Tenant, int> tenantrepository)//, TenantManager tenantManager)
        {
            _tenantRepository = tenantrepository;
            //_tenantManager = tenantManager;
        }
        public async Task<List<Tenant>> GetAllAsync()
        {
            var tenant = await _tenantRepository.GetAllListAsync();

            if (tenant == null)
            {
                throw new UserFriendlyException("No tenant found!");
            }
            return tenant;
        }
        public async Task DeleteTenantAsync(int tenantId)
        {
            var tenant = await _tenantRepository.GetAsync(tenantId);

            if (tenant == null)
            {
                throw new UserFriendlyException("Tenant not found.");
            }
            if (tenant.DeletionTime.HasValue && tenant.DeletionTime.Value < DateTime.Now.Subtract(TimeSpan.FromDays(90)))
            {
                //await _tenantManager.DeleteAsync(tenant);
                await _tenantRepository.DeleteAsync(tenant);
            }
            else
            {
                throw new UserFriendlyException("Tenant deletion date is not set or not within the 90-day threshold.");
            }
        }
    }
}
