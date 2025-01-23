using Abp.Domain.Services;
using ERPack.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Preferences
{
    public interface IPreferenceManager : IDomainService
    {
        Task<Preference> GetAsync(int id);
        Task<int> CreateAsync(Preference preference);
        Task<Preference> UpdateAsync(Preference preference);
        void Cancel(Preference preference);
        Task<List<Preference>> GetAllAsync();
        Task<Preference> GetByIdTypeAsync(string idType);
    }
}
