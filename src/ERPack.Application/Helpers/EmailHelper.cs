using Abp.Logging;
using Abp.UI;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Mail;

namespace ERPack.Helpers
{
    /// <summary>
    /// Helper Class Related to Sending Email 
    /// </summary>
    public class EmailHelper : ERPackAppServiceBase, IEmailHelper
    {
        private readonly IHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor for Email Helper 
        /// </summary>
        public EmailHelper(IHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool SendEmail(string toEmailId, string subjectBody, string messageBody)
        {
            try
            {      
                MailMessage msg = new MailMessage();
                msg.To.Add(toEmailId);
                msg.From = new MailAddress("mayurtheonetech@gmail.com");
                msg.Subject = subjectBody;
                msg.Body = messageBody;
                msg.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient()
                {   Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("mayurtheonetech@gmail.com", "tzfq etno vvpi fibc"),
                    EnableSsl = true // Set to true if your server requires SSL,                    
                };
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message);
                return false;
            }
            return true;
        }

        public string GetEstimateTemplate(string estimateNumber, long enquiryId)
        {
            try
            {
                //Fetching Email Body Text from EmailTemplate File.  
                var FilePath = Path.Combine(_env.ContentRootPath, "wwwroot\\EmailTemplates\\ApproveEstimate.html");

                var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

                StreamReader str = new StreamReader(fs);
                string MailText = str.ReadToEnd();
                str.Close();
                string host = _httpContextAccessor.HttpContext.Request.Host.ToString();
                string scheme = _httpContextAccessor.HttpContext.Request.Scheme;

                string url = scheme + "://" + host;

                MailText = MailText.Replace("{{EnquiryId}}", enquiryId.ToString());
                MailText = MailText.Replace("{{EstimateNumber}}", estimateNumber);
                MailText = MailText.Replace("{{ApproveLink}}",
                           url + "/CRM/ApproveEstimateByEmail?enquiryId=" + enquiryId + "&isApproved=" + true);
                MailText = MailText.Replace("{{RejectLink}}",
                           url + "/CRM/ApproveEstimateByEmail?enquiryId=" + enquiryId + "&isApproved=" + false);

                return MailText;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Could not found the email template!",ex.Message);
            }

        }

        public string GetTenantResetPasswordTemplate(string userName, string resetLink)
        {
            try
            {
                //Fetching Email Body Text from EmailTemplate File.  
                var FilePath = Path.Combine(_env.ContentRootPath, "wwwroot\\EmailTemplates\\TenantResetPassword.html");

                var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

                StreamReader str = new StreamReader(fs);
                string MailText = str.ReadToEnd();
                str.Close();
                string host = _httpContextAccessor.HttpContext.Request.Host.ToString();
                string scheme = _httpContextAccessor.HttpContext.Request.Scheme;

                string url = scheme + "://" + host;

                MailText = MailText.Replace("{{UserName}}", userName);
                MailText = MailText.Replace("{{ResetLink}}", resetLink);

                return MailText;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Could not found the email template!", ex.Message);
            }

        }
    }
}
