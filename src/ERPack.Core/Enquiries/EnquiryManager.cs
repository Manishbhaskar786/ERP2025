using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Enquiries
{
    public class EnquiryManager : IEnquiryManager
    {
        private readonly IRepository<Enquiry, long> _enquiryRepository;

        public EnquiryManager(IRepository<Enquiry, long> enquiryRepository)
        {
            _enquiryRepository = enquiryRepository;
        }

        public async Task<long> CreateAsync(Enquiry enquiry)
        {
            return await _enquiryRepository.InsertAndGetIdAsync(enquiry);

        }

        public async Task<Enquiry> UpdateAsync(Enquiry enquiry)
        {
            return await _enquiryRepository.UpdateAsync(enquiry);
        }

        public async Task<Enquiry> GetAsync(long id)
        {
            var enquiry = await _enquiryRepository.GetAll().AsNoTracking()
                .Include(x => x.Customer).Include(x => x.BoardType).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (enquiry == null)
            {
                throw new UserFriendlyException("Could not found the enquiry, maybe it's deleted!");
            }
            return enquiry;

        }

        public async Task<List<Enquiry>> GetAllAsync()
        {
            var enquires = await _enquiryRepository.GetAll().ToListAsync();

            if (enquires == null)
            {
                throw new UserFriendlyException("No enquires found, please contact admin!");
            }
            return enquires;
        }

        public void Cancel(Enquiry enquiry)
        {
            _enquiryRepository.Delete(enquiry);
        }

        public async Task DeleteAsync(Enquiry enquiry)
        {
            var result = await _enquiryRepository
                .FirstOrDefaultAsync(x => x.Id == enquiry.Id)
                ?? throw new UserFriendlyException("No enquiry found with the ID. Please contact admin!");

            result.IsDeleted = true;
            result.DeletionTime = DateTime.Now;

            await _enquiryRepository.UpdateAsync(result);

        }

    }
}
