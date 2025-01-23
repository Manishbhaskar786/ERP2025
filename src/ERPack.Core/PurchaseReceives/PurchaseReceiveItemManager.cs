using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.PurchaseRecieves;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.PurchaseOrders
{
    public class PurchaseReceiveItemManager : IPurchaseReceiveItemManager
    {
        private readonly IRepository<PurchaseReceiveItem, int> _repository;

        public PurchaseReceiveItemManager(
            IRepository<PurchaseReceiveItem, int> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(PurchaseReceiveItem purchaseReceiveItem)
        {
            return await _repository.InsertAndGetIdAsync(purchaseReceiveItem);

        }

        public async Task<PurchaseReceiveItem> GetAsync(int id)
        {
            var purchaseReceiveItem = await _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (purchaseReceiveItem == null)
            {
                throw new UserFriendlyException("Could not found the purchaseReceiveItem, maybe it's deleted!");
            }
            return purchaseReceiveItem;
        }

        public async Task<List<PurchaseReceiveItem>> GetAllByPurchaseRecieveIdAsync(int id)
        {
            var purchaseReceiveItems = await _repository.GetAll()
                .Include(x=> x.PurchaseOrderItem).ThenInclude(z=> z.Unit)
                .Include(z=> z.PurchaseOrderItem).ThenInclude(z=> z.Material)
                .Include(x=> x.Store)
                .Where(x => x.PurchaseReceiveId == id).ToListAsync();

            if (purchaseReceiveItems == null)
            {
                throw new UserFriendlyException("No purchaseReceiveItems found, maybe it's deleted!");
            }
            return purchaseReceiveItems;

        }

        public async Task<List<PurchaseReceiveItem>> GetAllAsync()
        {
            var purchaseReceiveItems = await _repository.GetAll().ToListAsync();

            if (purchaseReceiveItems == null)
            {
                throw new UserFriendlyException("No purchaseReceiveItems found, please contact admin!");
            }
            return purchaseReceiveItems;

        }

        public void Cancel(PurchaseReceiveItem purchaseReceiveItem)
        {
            _repository.Delete(purchaseReceiveItem);
        }
    }
}
