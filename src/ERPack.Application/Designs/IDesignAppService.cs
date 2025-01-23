using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERPack.Common.Dto;
using ERPack.Designs.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.Designs
{
    public interface IDesignAppService : IApplicationService
    {
        Task<long> CreateAsync(DesignDto input);
        Task<DesignDto> UpdateAsync(DesignDto input);
        Task<DesignDto> GetAsync(long designId);
        Task<DesignDto> UpdateStatusAsync(long designId, string status);
        Task<List<DesignSheet>> GetDesignNamesAsync(string name);
        Task<PagedResultDto<DesignDto>> GetCompletedDesignsAsync(CommonPagedResultRequestDto input);
        Task<List<DesignDto>> GetAllCompletedDesignsAsync();
        Task<List<ToolConfiguration>> GetToolConfigurationsAsync();
        Task<List<BoardType>> GetBoardTypesAsync(string query);
        Task<long> AddDesignMaterialsAsync(DesignMaterialDto input);
        Task<DesignMaterialDto> UpdateDesignMaterialAsync(DesignMaterialDto input);
        Task<List<DesignMaterialDto>> GetDesignMaterialsAsync(long designId);
        Task<DesignMaterialDto> GetDesignMaterialAsync(int id);
        Task DeleteDesignMaterialAsync(int id);
    }
}
