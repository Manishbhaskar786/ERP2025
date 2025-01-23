using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.PurchaseIndents
{
    public class PurchaseIndentManager : IPurchaseIndentManager
    {
        private readonly IRepository<PurchaseIndent, long> _repository;

        public PurchaseIndentManager(
            IRepository<PurchaseIndent, long> repository)
        {
            _repository = repository;
        }

        public async Task<long> CreateAsync(PurchaseIndent purchaseIndent)
        {
            return await _repository.InsertAndGetIdAsync(purchaseIndent);

        }

        public async Task<PurchaseIndent> GetAsync(long id)
        {
            var purchaseIndent = await _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (purchaseIndent == null)
            {
                throw new UserFriendlyException("Could not found the purchaseIndent, maybe it's deleted!");
            }
            return purchaseIndent;

        }

        public async Task<List<PurchaseIndent>> GetAllAsync()
        {
            var purchaseIndents = await _repository.GetAll().ToListAsync();

            if (purchaseIndents == null)
            {
                throw new UserFriendlyException("No purchaseIndent found, please contact admin!");
            }
            return purchaseIndents;

        }

        public void Cancel(PurchaseIndent purchaseIndent)
        {
            _repository.Delete(purchaseIndent);
        }
    }
}
