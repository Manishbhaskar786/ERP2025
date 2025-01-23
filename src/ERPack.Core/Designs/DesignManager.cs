using Abp.Domain.Repositories;
using Abp.UI;
using ERPack.Enquiries;
using ERPack.Materials;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Designs
{
    public class DesignManager : IDesignManager
    {
        private readonly IRepository<Design, long> _designRepository;
        private readonly IRepository<DesignSheet, int> _designSheetRepository;
        private readonly IRepository<ToolConfiguration, int> _toolConfigurationRepository;
        private readonly IRepository<BoardType, int> _boardTypeRepository;

        public DesignManager(IRepository<DesignSheet, int> designSheetRepository,
            IRepository<Design, long> designRepository,
            IRepository<ToolConfiguration, int> toolConfigurationRepository,
            IRepository<BoardType, int> boardTypeRepository)
        {
            _designRepository = designRepository;
            _designSheetRepository = designSheetRepository;
            _toolConfigurationRepository = toolConfigurationRepository;
            _boardTypeRepository = boardTypeRepository;
        }


        public async Task<long> CreateAsync(Design design)
        {
            return await _designRepository.InsertAndGetIdAsync(design);

        }

        public async Task<Design> UpdateAsync(Design design)
        {
            return await _designRepository.UpdateAsync(design);
        }

        public async Task<Design> GetAsync(long id)
        {
            var design = await _designRepository
                .GetAll()
                .Include(x => x.Enquiry)
                .ThenInclude(x => x.BoardType)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (design == null)
            {
                throw new UserFriendlyException("Could not found the design, maybe it's deleted!");
            }
            return design;
        }

        public async Task<List<Design>> GetAllAsync()
        {
            var designs = await _designRepository.GetAll().ToListAsync();

            if (designs == null)
            {
                throw new UserFriendlyException("No designs found, please contact admin!");
            }
            return designs;
        }

        public void Cancel(Design design)
        {
            _designRepository.Delete(design);
        }

        public async Task<List<DesignSheet>> GetDesignNamesAsync(string query)
        {
            return await _designSheetRepository.GetAll().Where(x => x.DesignName.Contains(query) ||
                x.DesignNumber.Contains(query)).ToListAsync();
        }

        public async Task<List<ToolConfiguration>> GetToolConfigurationsAsync()
        {
            var materials = await _toolConfigurationRepository.GetAll().ToListAsync();

            if (materials == null)
            {
                throw new UserFriendlyException("No Tool Configurations found, please contact admin!");
            }
            return materials;

        }

        public async Task<List<BoardType>> GetBoardTypesAsync(string query)
        {
            return await _boardTypeRepository.GetAll().Where(x => x.BoardTypeName.Contains(query)).ToListAsync();
        }

    }
}
