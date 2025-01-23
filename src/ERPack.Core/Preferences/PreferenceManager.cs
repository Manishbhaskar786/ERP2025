using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Preferences
{
    public class PreferenceManager : IPreferenceManager
    {
        private readonly IRepository<Preference, int> _repository;

        public PreferenceManager(
            IRepository<Preference, int> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(Preference preference)
        {
            return await _repository.InsertAndGetIdAsync(preference);
        }

        public async Task<Preference> UpdateAsync(Preference preference)
        {
            return await _repository.UpdateAsync(preference);
        }

        public Task<Preference> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                var store = _repository.GetAll().Where(x => x.Id == id).FirstOrDefault();

                if (store == null)
                {
                    throw new UserFriendlyException("Could not found the preference, maybe it's deleted!");
                }
                return store;
            });
        }

        public Task<Preference> GetByIdTypeAsync(string idType)
        {
            return Task.Run(() =>
            {
                var preference = _repository.GetAll().Where(x => x.IdType == idType).FirstOrDefault();
                return preference;
            });
        }

        public async Task<List<Preference>> GetAllAsync()
        {
            var preferences = await _repository.GetAll().ToListAsync();

            if (preferences == null)
            {
                throw new UserFriendlyException("No preferences found, please contact admin!");
            }
            return preferences;

        }

        public void Cancel(Preference preference)
        {
            try
            {
                _repository.Delete(preference);
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("Error deleting preference", ex.Message);
            }

        }
    }
}
