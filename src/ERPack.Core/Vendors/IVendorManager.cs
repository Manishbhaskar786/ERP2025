using Abp.Domain.Services;
using ERPack.Vendors;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Stores
{
    public interface IVendorManager : IDomainService
    {
        Task<Vendor> GetAsync(int id);
        Task<int> CreateAsync(Vendor vendor);
        void Cancel(Vendor vendor);
        Task<List<Vendor>> GetAllAsync();
    }
}
