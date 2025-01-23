using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Enquiries
{
    public interface IEnquiryManager : IDomainService
    {
        Task<long> CreateAsync(Enquiry enquiry);
        Task<Enquiry> UpdateAsync(Enquiry enquiry);
        Task<Enquiry> GetAsync(long id);
        Task<List<Enquiry>> GetAllAsync();
        void Cancel(Enquiry enquiry);
        Task DeleteAsync(Enquiry enquiry);
    }
}
