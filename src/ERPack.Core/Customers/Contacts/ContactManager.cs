using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Customers.Contacts
{
    public class ContactManager : IContactManager
    {
        private readonly IRepository<Contact, long> _repository;

        public ContactManager(
            IRepository<Contact, long> repository)
        {
            _repository = repository;
        }

        public async Task<long> CreateAsync(Contact contact)
        {
            return await _repository.InsertAndGetIdAsync(contact);
        }

        public async Task<Contact> UpdateAsync(Contact contact)
        {
            return await _repository.UpdateAsync(contact);
        }

        public async Task<Contact> GetAsync(long id)
        {
            var contact = await _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (contact == null)
            {
                throw new UserFriendlyException("Could not found the contact, maybe it's deleted!");
            }
            return contact;

        }

        public async Task<Contact> CheckUniquenessAsync(string email, long id = 0)
        {
            var contact = await _repository.GetAll().Where(x => x.EmailAddress.ToLower() == email.ToLower() && (id == 0 || x.Id != id)).FirstOrDefaultAsync();
            return contact;
        }
    }
}
