using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.PurchaseOrders
{
    public class PurchaseOrderManager : IPurchaseOrderManager
    {
        private readonly IRepository<PurchaseOrder, int> _repository;

        public PurchaseOrderManager(
            IRepository<PurchaseOrder, int> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(PurchaseOrder purchaseOrder)
        {
            return await _repository.InsertAndGetIdAsync(purchaseOrder);

        }

        public async Task<int> UpdateAsync(PurchaseOrder purchaseOrder)
        {
            return await _repository.InsertOrUpdateAndGetIdAsync(purchaseOrder);

        }

        public async Task<PurchaseOrder> GetAsync(int id)
        {
            var purchaseOrder = await _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (purchaseOrder == null)
            {
                throw new UserFriendlyException("Could not found the purchaseOrder, maybe it's deleted!");
            }
            return purchaseOrder;
        }

        public async Task<List<PurchaseOrder>> GetAllAsync()
        {
            var purchaseOrders = await _repository.GetAll().ToListAsync();

            if (purchaseOrders == null)
            {
                throw new UserFriendlyException("No purchaseOrder found, please contact admin!");
            }
            return purchaseOrders;

        }

        public void Cancel(PurchaseOrder purchaseOrder)
        {
            _repository.Delete(purchaseOrder);
        }
    }
}
