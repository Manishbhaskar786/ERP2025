using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.PurchaseRecieves;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.PurchaseReceives
{
    public class PurchaseReceiveManager : IPurchaseReceiveManager
    {
        private readonly IRepository<PurchaseReceive, int> _repository;

        public PurchaseReceiveManager(
            IRepository<PurchaseReceive, int> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(PurchaseReceive purchaseReceive)
        {
            try
            {
                return await _repository.InsertAndGetIdAsync(purchaseReceive);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error Creating Purchase Receives", ex.Message);
            }
        }

        public async Task<PurchaseReceive> GetAsync(int id)
        {
            var purchaseReceive = await _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (purchaseReceive == null)
            {
                throw new UserFriendlyException("Could not found the purchaseReceive, maybe it's deleted!");
            }
            return purchaseReceive;

        }

        public async Task<List<PurchaseReceive>> GetAllAsync()
        {
            var purchaseReceives = await _repository.GetAll().ToListAsync();

            if (purchaseReceives == null)
            {
                throw new UserFriendlyException("No purchaseReceives found, please contact admin!");
            }
            return purchaseReceives;

        }

        public void Cancel(PurchaseReceive purchaseReceive)
        {
            _repository.Delete(purchaseReceive);
        }
    }
}
