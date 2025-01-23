using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Vendors;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Stores
{
    public class VendorManager : IVendorManager
    {
        private readonly IRepository<Vendor, int> _vendorRepository;

        public VendorManager(
            IRepository<Vendor, int> vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<int> CreateAsync(Vendor vendor)
        {
            return await _vendorRepository.InsertAndGetIdAsync(vendor);

        }

        public async Task<Vendor> UpdateAsync(Vendor vendor)
        {
            return await _vendorRepository.UpdateAsync(vendor);
        }

        public Task<Vendor> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                var vendor = _vendorRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();

                if (vendor == null)
                {
                    throw new UserFriendlyException("Could not found the vendor, maybe it's deleted!");
                }
                return vendor;
            });
        }

        public async Task<List<Vendor>> GetAllAsync()
        {
            var vendors = await _vendorRepository.GetAll().ToListAsync();

            if (vendors == null)
            {
                throw new UserFriendlyException("No vendors found, please contact admin!");
            }
            return vendors;

        }

        public void Cancel(Vendor vendor)
        {
            _vendorRepository.Delete(vendor);
        }
    }
}
