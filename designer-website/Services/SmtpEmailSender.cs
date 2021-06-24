using designer_website.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace designer_website.Services
{
    public class SmtpEmailSender : ISmtpEmailSender
    {
        private readonly IConfiguration Configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
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
                client.Connect(Configuration["SmtpConfiguration:SmtpServer"], 465, true);
                client.Authenticate(Configuration["SmtpConfiguration:SmtpUser"], Configuration["SmtpConfiguration:SmtpPassword"]);
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