using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Stores
{
    public interface IStoreManager : IDomainService
    {
        Task<Store> GetAsync(int id);
        Task<int> CreateAsync(Store store);
        Task<Store> UpdateAsync(Store store);
        void Cancel(Store store);
        Task<List<Store>> GetAllAsync();
    }
}
