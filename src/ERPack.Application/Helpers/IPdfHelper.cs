using Abp.Application.Services;
using Abp.Domain.Services;
using ERPack.Sessions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Helpers
{
    public interface IPdfHelper : IApplicationService
    {
        Task<byte[]> GetWorkorder(long workorderId, string imageURL);
        Task<byte[]> GetEstimate(long estimateId, GetCurrentLoginInformationsOutput loginInfo);
        Task<byte[]> GetDesignJobCard(long enquiryId, GetCurrentLoginInformationsOutput loginInfo);
        Byte[] ExportTable(string title, string data);
    }
}
