using Abp.Domain.Services;
using System.Threading.Tasks;

namespace ERPack.Customers.Contacts
{
    public interface IContactManager : IDomainService
    {
        Task<Contact> GetAsync(long id);
        Task<long> CreateAsync(Contact contact);
        Task<Contact> CheckUniquenessAsync(string email, long id = 0);
    }
}
