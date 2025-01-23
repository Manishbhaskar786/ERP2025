using Abp.Application.Services;
using ERPack.PurchaseIndents.Dto;
using ERPack.PurchaseOrders.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.PurchaseIndents
{
    public interface IPurchaseIndentAppService : IApplicationService
    {
        Task<long> CreateAsync(PurchaseIndentDto input);
    }
}
