using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace DropMate.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;

        public EmailService(IConfiguration configuration, ILoggerManager logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void SendEmail(string toAddress, string subject, string body)
        {
            string smtpHost = _configuration["EmailConfiguration:Host"];
            int smtpPort = int.Parse(_configuration["EmailConfiguration:Port"]);
            string smtpUsername = _configuration["EmailConfiguration:Username"];
            string smtpPassword = _configuration["EmailConfiguration:Password"];
            bool enableSsl = bool.Parse(_configuration["EmailConfiguration:EnableSsl"]);

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = enableSsl;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpUsername);
                mailMessage.To.Add(toAddress);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                try
                {
                    smtpClient.Send(mailMessage);

                }catch (Exception ex)
                {
                    _logger.LogError($"Failed to send email at {DateTime.Now}. Here are details of the error\n {ex}");
                }
            }
        }
    }
}
