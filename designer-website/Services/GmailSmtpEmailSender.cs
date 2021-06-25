using designer_website.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace designer_website.Services
{
    public class GmailSmtpEmailSender : ISmtpEmailSender
    {
        private readonly IConfiguration _configuration;

        public GmailSmtpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public EmailResult TryToSendMail(MailboxAddress @from, MailboxAddress to, string subject, MimeEntity body)
        {
            MimeMessage message = new MimeMessage();
            
            message.From.Add(from);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;

            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect(_configuration["SmtpConfiguration:SmtpServer"], 465, true);
                client.Authenticate(_configuration["SmtpConfiguration:SmtpUser"], _configuration["SmtpConfiguration:SmtpPassword"]);
                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
            }
            catch
            {
                return EmailResult.SendFail;
            }

            return EmailResult.SendSuccess;
        }
    }
}