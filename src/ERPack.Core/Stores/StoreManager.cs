using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Stores
{
    public class StoreManager : IStoreManager
    {
        private readonly IRepository<Store, int> _storeRepository;

        public StoreManager(
            IRepository<Store, int> storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<int> CreateAsync(Store store)
        {
            return await _storeRepository.InsertAndGetIdAsync(store);
        }

        public async Task<Store> UpdateAsync(Store store)
        {
            return await _storeRepository.UpdateAsync(store);
        }

        public Task<Store> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                var store = _storeRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();

                if (store == null)
                {
                    throw new UserFriendlyException("Could not found the store, maybe it's deleted!");
                }
                return store;
            });
        }

        public async Task<List<Store>> GetAllAsync()
        {
            var stores = await _storeRepository.GetAll().ToListAsync();

            if (stores == null)
            {
                throw new UserFriendlyException("No stores found, please contact admin!");
            }
            return stores;

        }

        public void Cancel(Store store)
        {
            _storeRepository.Delete(store);
        }
    }
}
