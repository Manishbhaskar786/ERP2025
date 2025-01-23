using Abp.Domain.Services;
using ERPack.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Designs
{
    public interface IDesignManager : IDomainService
    {
        Task<long> CreateAsync(Design design);
        Task<Design> UpdateAsync(Design design);
        Task<Design> GetAsync(long id);
        Task<List<Design>> GetAllAsync();
        void Cancel(Design design);
        Task<List<DesignSheet>> GetDesignNamesAsync(string query);
        Task<List<ToolConfiguration>> GetToolConfigurationsAsync();
        Task<List<BoardType>> GetBoardTypesAsync(string query);
    }
}
