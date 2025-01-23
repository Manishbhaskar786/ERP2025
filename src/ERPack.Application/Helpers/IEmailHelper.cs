using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Helpers
{
    public interface IEmailHelper : IApplicationService
    {
        bool SendEmail(string toEmailId, string subjectBody, string messageBody);
        string GetEstimateTemplate(string estimateNumber, long enquiryId);
        string GetTenantResetPasswordTemplate(string userName, string resetLink);
    }
}
