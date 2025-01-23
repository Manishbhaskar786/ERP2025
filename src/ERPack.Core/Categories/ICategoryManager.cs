using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Categories
{
    public interface ICategoryManager : IDomainService
    {
        Task<Category> GetAsync(int id);
        Task<int> CreateAsync(Category @event);
        void Cancel(Category @event);
        Task<List<Category>> GetAllAsync();
    }
}
