using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Categories
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IRepository<Category, int> _categoryRepository;

        public CategoryManager(
            IRepository<Category, int> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<int> CreateAsync(Category category)
        {
            return await _categoryRepository.InsertAndGetIdAsync(category);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAll().ToListAsync();
            if (categories == null)
            {
                throw new UserFriendlyException("No Categories found, please contact admin!");
            }
            return categories;
        }

        public Task<Category> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                var category = _categoryRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();

                if (category == null)
                {
                    throw new UserFriendlyException("Could not found the category, maybe it's deleted!");
                }
                return category;
            });
        }

        public void Cancel(Category category)
        {
            _categoryRepository.Delete(category);
        }
    }
}
