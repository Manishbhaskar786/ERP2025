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

namespace ERPack.HostTaxation
{
    public class HostTaxationManager : IHostTaxationManager
    {
        private readonly IRepository<HostTaxationInfo, int> _hostTaxationRepository;

        public HostTaxationManager(
            IRepository<HostTaxationInfo, int> hostTenantRepository)
        {
            _hostTaxationRepository = hostTenantRepository;
        }

        public async Task<int> CreateAsync(HostTaxationInfo hostTaxationInfo)
        {
            return await _hostTaxationRepository.InsertAndGetIdAsync(hostTaxationInfo);
        }

        public async Task<HostTaxationInfo> UpdateAsync(HostTaxationInfo hostTaxationInfo)
        {
            return await _hostTaxationRepository.UpdateAsync(hostTaxationInfo);
        }

        public async Task<HostTaxationInfo> GetAsync(int hostTaxationId)
        {
            var material = await _hostTaxationRepository.GetAll().Where(x => x.Id == hostTaxationId).FirstOrDefaultAsync();

            if (material == null)
            {
                throw new UserFriendlyException("Could not found the host taxation info, maybe it's deleted!");
            }
            return material;
        }

        public async Task<List<HostTaxationInfo>> GetAllAsync()
        {
            var hostTaxationInfo = await _hostTaxationRepository.GetAll().ToListAsync();

            if (hostTaxationInfo == null)
            {
                throw new UserFriendlyException("No host taxation info found, please contact admin!");
            }
            return hostTaxationInfo;

        }
    }
}
