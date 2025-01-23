using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Categories;
using ERPack.Categories.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Departments
{
    [AbpAuthorize]
    public class CategoryAppService : ERPackAppServiceBase , ICategoryAppService
    {
        private readonly CategoryManager _categoryManager;
        readonly IRepository<Category, int> _repository;

        public CategoryAppService(IRepository<Category> repository,
            CategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
            _repository = repository;
        }

        public  async Task<(long, string)> CreateCategoryAsync(CategoryDto input)
        {
            try
            {
                var categoryQuery = _repository.GetAll().Where(x => !x.IsDeleted && x.CategoryName.Trim().ToLower() == input.CategoryName.Trim().ToLower() && (!input.TenantId.HasValue || x.TenantId == input.TenantId.Value));
                var existCategory = await categoryQuery.FirstOrDefaultAsync();
                if (existCategory != null)
                {
                    throw new UserFriendlyException("Category already exists.");
                }
                var category = ObjectMapper.Map<Category>(input);

                int categoryId = await _categoryManager.CreateAsync(category);

                return (categoryId, string.Empty);
            }
            catch (Exception ex)
            {
                return (0, ex.Message);
            }
        }

        public async Task<List<CategoryOutput>> GetAllAsync()
        {
            var categories = await _categoryManager.GetAllAsync();

            var result = ObjectMapper.Map<List<CategoryOutput>>(categories);

            return result;
        }
    }
}
