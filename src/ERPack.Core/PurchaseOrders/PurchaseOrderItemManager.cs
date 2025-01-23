using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.PurchaseOrders
{
    public class PurchaseOrderItemManager : IPurchaseOrderItemManager
    {
        private readonly IRepository<PurchaseOrderItem, int> _repository;

        public PurchaseOrderItemManager(
            IRepository<PurchaseOrderItem, int> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(PurchaseOrderItem purchaseOrderItem)
        {
            return await _repository.InsertAndGetIdAsync(purchaseOrderItem);

        }

        public async Task<int> UpdateAsync(PurchaseOrderItem purchaseOrderItem)
        {
            return await _repository.InsertOrUpdateAndGetIdAsync(purchaseOrderItem);

        }

        public async Task<PurchaseOrderItem> GetAsync(int id)
        {
            var purchaseOrderItem = await _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (purchaseOrderItem == null)
            {
                throw new UserFriendlyException("Could not found the purchaseOrderItem, maybe it's deleted!");
            }
            return purchaseOrderItem;
        }

        public async Task<List<PurchaseOrderItem>> GetAllByPurchaseOrderIdAsync(int id)
        {
            var purchaseOrderItems = await _repository.GetAll().Include(x=> x.PurchaseOrder).ThenInclude(x=> x.Vendor)
                .Include(x=> x.ItemType)
                .Include(x=> x.Material)
                .Include(x=> x.Unit)
                .Where(x => x.PurchaseOrderId == id).ToListAsync();

            if (purchaseOrderItems == null)
            {
                throw new UserFriendlyException("No purchaseOrderItems found, maybe it's deleted!");
            }
            return purchaseOrderItems;
        }

        public async Task<List<PurchaseOrderItem>> GetAllByPOCodeAsync(string poCode)
        {
            var purchaseOrderItems = await _repository.GetAll()
                .Include(x => x.Unit)
                .Include(x => x.Material)
                .Include(x => x.PurchaseOrder).ThenInclude(x=> x.Vendor)
                .Where(x => x.PurchaseOrder.POCode.Trim() == poCode.Trim()).ToListAsync();

            if (purchaseOrderItems == null)
            {
                throw new UserFriendlyException("No purchaseOrderItems found, maybe it's deleted!");
            }
            return purchaseOrderItems;
        }

        public async Task<List<PurchaseOrderItem>> GetAllAsync()
        {
            var purchaseOrderItems = await _repository.GetAll().ToListAsync();

            if (purchaseOrderItems == null)
            {
                throw new UserFriendlyException("No purchaseOrderItems found, please contact admin!");
            }
            return purchaseOrderItems;

        }

        public void Cancel(PurchaseOrderItem purchaseOrderItem)
        {
            _repository.Delete(purchaseOrderItem);
        }
    }
}
