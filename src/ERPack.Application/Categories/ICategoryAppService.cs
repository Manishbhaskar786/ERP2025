using Abp.Application.Services;
using ERPack.Categories.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Categories
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<(long, string)> CreateCategoryAsync(CategoryDto input);
        Task<List<CategoryOutput>> GetAllAsync();
    }
}
