using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Customers;
using ERPack.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.MultiTenancy
{
    public class HostTenantManager : IHostTenantManager
    {
        private readonly IRepository<HostTenantInfo, int> _hostTenantRepository;

        public HostTenantManager(
            IRepository<HostTenantInfo, int> hostTenantRepository)
        {
            _hostTenantRepository = hostTenantRepository;
        }

        public async Task<int> CreateAsync(HostTenantInfo hostTenantInfo)
        {
            return await _hostTenantRepository.InsertAndGetIdAsync(hostTenantInfo);
        }

        public async Task<HostTenantInfo> UpdateAsync(HostTenantInfo hostTenantInfo)
        {
            return await _hostTenantRepository.UpdateAsync(hostTenantInfo);
        }

        public async Task<HostTenantInfo> GetAsync()
        {
            var material = await _hostTenantRepository.GetAll().FirstOrDefaultAsync();

            if (material == null)
            {
                throw new UserFriendlyException("Could not found the host tenant info, maybe it's deleted!");
            }
            return material;
        }

        public async Task<List<HostTenantInfo>> GetAllAsync()
        {
            var hostTenantInfo = await _hostTenantRepository.GetAll().ToListAsync();

            if (hostTenantInfo == null)
            {
                throw new UserFriendlyException("No host tenant info found, please contact admin!");
            }
            return hostTenantInfo;

        }
    }
}
