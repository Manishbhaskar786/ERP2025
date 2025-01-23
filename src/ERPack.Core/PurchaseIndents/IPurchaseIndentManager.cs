using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPack.PurchaseIndents
{
    public interface IPurchaseIndentManager : IDomainService
    {
        Task<PurchaseIndent> GetAsync(long id);
        Task<long> CreateAsync(PurchaseIndent purchaseIndent);
        void Cancel(PurchaseIndent purchaseIndent);
        Task<List<PurchaseIndent>> GetAllAsync();
    }
}
